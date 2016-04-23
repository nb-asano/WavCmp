/**
 * Copyright (c) 2012-2013 Sakura-Zen soft All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR(S) ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR(S) BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PcmLib
{
	public class RiffReader : IPcmStream, IDisposable
	{
		public struct Chunk
		{
			public string fourCC;
			public long offset;
			public long size;
		}

		/// <summary>解析対したWaveFormatヘッダー</summary>
		private WaveFormatEx waveFormat;
		/// <summary>ファイルのストリーム開始バイトオフセット</summary>
		private long streamOffset;
		/// <summary>ファイルのストリームバイトサイズ</summary>
		private long streamLength;
		/// <summary>ストリーム読み込み用のファイルストリーム</summary>
		private FileStream streamFs;
		/// <summary>ストリーム読み込み用のBinaryReader</summary>
		private BinaryReader streamBr;
		/// <summary>拡張チャンク情報</summary>
		private List<Chunk> chunks;

		/// <summary>解析対象ファイルパス</summary>
		public string FilePath
		{
			get { return streamFs.Name; }
		}
		/// <summary>解析したWaveFormatヘッダー</summary>
		public WaveFormatEx WaveFormat
		{
			get { return waveFormat; }
		}
		/// <summary>ストリームの有効バイトサイズ</summary>
		public long Length
		{
			get { return streamLength; }
		}
		/// <summary>ストリームの有効サンプル数</summary>
		public long Samples
		{
			get
			{
				if (waveFormat.BitsPerSample == 0) {
					return 0;
				}
				return streamLength / (waveFormat.BitsPerSample / 8);
			}
		}
		/// <summary>ストリームの有効ブロック数</summary>
		public long Blocks
		{
			get
			{
				if (waveFormat.BlockAlign == 0) {
					return 0;
				}
				return streamLength / waveFormat.BlockAlign;
			}
		}
		/// <summary>ストリームの現在の読み込みバイトオフセット</summary>
		public long CurrentPos
		{
			get { return (streamFs.Position - streamOffset); }
		}
		/// <summary>ストリーム開始位置のバイトオフセット</summary>
		public long StreamOffset
		{
			get { return streamOffset; }
		}
		/// <summary>ファイル内に含まれる拡張チャンク情報</summary>
		public List<Chunk> Chunks
		{
			get { return chunks; }
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="path">ファイルのパス</param>
		public RiffReader(string path)
		{
			waveFormat = new WaveFormatEx();
			streamOffset = streamLength = 0;
			chunks = new List<Chunk>();

			if (!File.Exists(path)) {
				throw new IOException("no file");
			}

			try {
				streamFs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
				streamBr = new BinaryReader(streamFs);
			} catch (Exception) {   // 乱暴だがこれで問題になる状況はよほどの状況
				Close();
				throw;
			}

			InitializeDisposeFinalizePattern();
		}

		/// <summary>
		/// ファイルの解析を行います。
		/// </summary>
		/// <returns>解析に成功すれば真</returns>
		public bool Parse()
		{
			try {
				byte[] b = streamBr.ReadBytes(4);
				if (b[0] != 'R' || b[1] != 'I' || b[2] != 'F' || b[3] != 'F') {
					return false;
				}

				// 全体サイズは読み込みサイズで判定するので無視
				streamBr.BaseStream.Seek(4, SeekOrigin.Current);

				b = streamBr.ReadBytes(4);
				if (b[0] != 'W' || b[1] != 'A' || b[2] != 'V' || b[3] != 'E') {
					return false;
				}

				// WAVEチャンク内にfmtチャンクが見つかるまでチャンクを読み捨てる
				while (true) {
					b = streamBr.ReadBytes(4);
					if (b[0] == 'f' && b[1] == 'm' && b[2] == 't' && b[3] == ' ') {
						break;
					} else {
						long n = streamBr.ReadUInt32();
						streamBr.BaseStream.Seek(n, SeekOrigin.Current);
					}
				}

				// ヘッダー読み込み
				long cksize = streamBr.ReadUInt32();
				short tag = streamBr.ReadInt16();

				waveFormat.FormatTag = tag;
				waveFormat.Channels = streamBr.ReadInt16();
				waveFormat.SamplesPerSecond = streamBr.ReadInt32();
				waveFormat.AverageBytesPerSecond = streamBr.ReadInt32();
				waveFormat.BlockAlign = streamBr.ReadInt16();
				waveFormat.BitsPerSample = streamBr.ReadInt16();

				if (cksize > 16) {
					if (cksize == 40 && waveFormat.FormatTag == WaveFormatTag.EXTENSIBLE) {
						short size = streamBr.ReadInt16();
						if (size != 22) {
							return false;
						}
						waveFormat.Extensible = true;
						waveFormat.Samples = streamBr.ReadInt16();
						waveFormat.ChannelMask = streamBr.ReadUInt32();
						waveFormat.SubFormat = streamBr.ReadBytes(16);
					} else {
						// 不要データの読み飛ばし
						streamBr.BaseStream.Seek(cksize - 16, SeekOrigin.Current);
					}
				}

				// ストリーム開始位置（data Chunk）の走査
				while (true) {
					b = streamBr.ReadBytes(4);
					if (b[0] == 'd' && b[1] == 'a' && b[2] == 't' && b[3] == 'a') {
						break;
					} else {
						Chunk c = new Chunk();
						c.offset = streamBr.BaseStream.Position - 4;
						c.size = streamBr.ReadInt32();
						try {
							c.fourCC = Encoding.ASCII.GetString(b);
						} catch (DecoderFallbackException) {
							c.fourCC = "Unknown";
						}
						chunks.Add(c);
						// データの読み飛ばし
						streamBr.BaseStream.Seek(c.size, SeekOrigin.Current);
					}
				}

				// 有効ストリームサイズ
				cksize = streamBr.ReadUInt32();
				streamOffset = streamBr.BaseStream.Position;
				if (streamOffset + cksize > streamBr.BaseStream.Length) {
					streamLength = streamBr.BaseStream.Length - streamOffset;
				} else {
					streamLength = cksize;
				}

				// ストリーム先頭へ
				SeekStream(0, SeekOrigin.Begin);

			} catch (Exception) {   // 乱暴だがこれで問題になる状況はよほどの状況
				return false;
			}
			return true;
		}

		public byte[] Read8(int n)
		{
			if (streamFs.Length - streamFs.Position > n) {
				n = (int)(streamFs.Length - streamFs.Position);
			}
			return streamBr.ReadBytes(n);
		}

		public short[] Read16(int n)
		{
			int size = n * 2;
			if (streamFs.Length - streamFs.Position > size) {
				size = (int)(streamFs.Length - streamFs.Position);
			}
			size = (size / 2) * 2;	// 終端で出る端数は切り捨て

			byte[] b = streamBr.ReadBytes(size);
			return ByteArrayToShortArray(b);
		}

		public int[] Read24(int n)
		{
			int size = n * 3;
			if (streamFs.Length - streamFs.Position > size) {
				size = (int)(streamFs.Length - streamFs.Position);
			}
			size = (size / 3) * 3;	// 終端で出る端数は切り捨て

			byte[] b = streamBr.ReadBytes(size);

			// 32bit単位にパディングする
			int aligned = (b.Length / 3) * 4;
			byte[] ba = new byte[aligned];

			for (int i = 0, j = 0; i < size; i += 3, j += 4) {
				Array.Copy(b, i, ba, j, 3);
			}

			return ByteArrayToIntArray(ba);
		}

		public int[] Read32(int n)
		{
			int size = n * 4;
			if (streamFs.Length - streamFs.Position > size) {
				size = (int)(streamFs.Length - streamFs.Position);
			}
			size = (size / 4) * 4;	// 終端で出る端数は切り捨て

			byte[] b = streamBr.ReadBytes(size);
			return ByteArrayToIntArray(b);
		}

		public float[] Read32F(int n)
		{
			int size = n * 4;
			if (streamFs.Length - streamFs.Position > size) {
				size = (int)(streamFs.Length - streamFs.Position);
			}
			size = (size / 4) * 4;	// 終端で出る端数は切り捨て

			byte[] b = streamBr.ReadBytes(size);
			return ByteArrayToFloatArray(b);
		}

		public long SeekStream(long n, SeekOrigin origin)
		{
			if (origin == SeekOrigin.Begin) {
				if (n < 0 || n > streamLength) {
					return -1;
				}
				return streamFs.Seek(n + streamOffset, origin);
			} else if (origin == SeekOrigin.Current) {
				if (n + streamFs.Position < streamOffset || streamOffset + streamLength >= n + streamFs.Position) {
					return -1;
				}
				return streamFs.Seek(n, origin);
			}
			return -1;
		}

		/// <summary>
		/// ファイルをクローズします。
		/// </summary>
		public void Close()
		{
			Dispose();
		}

		#region Dispose Finalize Pattern

		/// <summary>
		/// 既にDisposeメソッドが呼び出されているかどうかを表します。
		/// </summary>
		private bool disposed;

		/// <summary>
		/// このクラスのインスタンスによって使用されているすべてのリソースを解放します。
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.Dispose(true);
		}

		/// <summary>
		/// このクラスのインスタンスがGCに回収される時に呼び出されます。
		/// </summary>
		~RiffReader()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// RiffWriter によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
		/// </summary>
		/// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。 </param>
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed) {
				return;
			}
			this.disposed = true;

			if (disposing) {
				// マネージ リソースの解放処理をこの位置に記述します。
				if (streamFs != null) {
					streamFs.Close();
					streamFs = null;
				}
				if (streamBr != null) {
					streamBr.Close();
					streamBr = null;
				}
			}
			// アンマネージ リソースの解放処理をこの位置に記述します。
		}

		/// <summary>
		/// 既にDisposeメソッドが呼び出されている場合、例外をスローします。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException">既にDisposeメソッドが呼び出されています。</exception>
		protected void ThrowExceptionIfDisposed()
		{
			if (this.disposed) {
				throw new ObjectDisposedException(this.GetType().FullName);
			}
		}

		/// <summary>
		/// Dispose Finalize パターンに必要な初期化処理を行います。
		/// </summary>
		private void InitializeDisposeFinalizePattern()
		{
			this.disposed = false;
		}

		#endregion

		#region Object派生

		/// <summary>
		/// ハッシュコードを取得します。オープンしているファイルに基づくハッシュコードです。
		/// </summary>
		/// <returns>ハッシュ値</returns>
		public override int GetHashCode()
		{
			return FilePath.GetHashCode();
		}
		/// <summary>
		/// 文字列表現を取得します。
		/// </summary>
		/// <returns>文字列表現</returns>
		public override string ToString()
		{
			return FilePath;
		}

		#endregion

		#region unsafeコード

		/// <summary>
		/// バイト配列をshort配列に変換します。奇数個のデータを与えることはできません。
		/// </summary>
		/// <param name="src">変換元のバイト配列</param>
		/// <returns>変換したshort配列</returns>
		/// <exception cref="InvalidDataException">データが奇数</exception>
		unsafe private short[] ByteArrayToShortArray(byte[] src)
		{
			if ((src.Length % 2) != 0) {
				throw new InvalidDataException();
			}
			short[] dest = new short[src.Length / 2];

			fixed (short* pDest = &dest[0]) {
				fixed (byte* pSrc = &src[0]) {
					CopyMemory(pDest, pSrc, (uint)src.Length);
				}
			}

			return dest;
		}
		/// <summary>
		/// バイト配列をint配列に変換します。4の倍数でない個数のデータを与えることはできません。
		/// </summary>
		/// <param name="src">変換元のバイト配列</param>
		/// <returns>変換したint配列</returns>
		/// <exception cref="InvalidDataException">データ数が4の倍数でない</exception>
		unsafe private int[] ByteArrayToIntArray(byte[] src)
		{
			if ((src.Length % 4) != 0) {
				throw new InvalidDataException();
			}
			int[] dest = new int[src.Length / 4];

			fixed (int* pDest = &dest[0]) {
				fixed (byte* pSrc = &src[0]) {
					CopyMemory(pDest, pSrc, (uint)src.Length);
				}
			}

			return dest;
		}
		/// <summary>
		/// バイト配列をfloat配列に変換します。4の倍数でない個数のデータを与えることはできません。
		/// </summary>
		/// <param name="src">変換元のバイト配列</param>
		/// <returns>変換したfloat配列</returns>
		/// <exception cref="InvalidDataException">データ数が4の倍数でない</exception>
		unsafe private float[] ByteArrayToFloatArray(byte[] src)
		{
			if ((src.Length % 4) != 0) {
				throw new InvalidDataException();
			}
			float[] dest = new float[src.Length / 4];

			fixed (float* pDest = &dest[0]) {
				fixed (byte* pSrc = &src[0]) {
					CopyMemory(pDest, pSrc, (uint)src.Length);
				}
			}

			return dest;
		}
		/// <summary>
		/// 配列間のデータの直接コピー。Cのmemcpy相当。
		/// </summary>
		/// <param name="outDest">出力配列のポインタ</param>
		/// <param name="inSrc">入力配列のポインタ</param>
		/// <param name="inNumOfBytes">コピーするバイトサイズ</param>
		unsafe private void CopyMemory(void* outDest, void* inSrc, uint inNumOfBytes)
		{
			// 転送先をuint幅にalignする
			const uint align = sizeof(uint) - 1;
			uint offset = (uint)outDest & align;
			// ↑ポインタは32bitとは限らないので本来このキャストはuintではダメだが、
			// 今は下位2bitだけあればいいのでこれでOK。
			if (offset != 0) {
				offset = align - offset;
			}
			offset = System.Math.Min(offset, inNumOfBytes);

			// 先頭の余り部分をbyteでちまちまコピー
			byte* srcBytes = (byte*)inSrc;
			byte* dstBytes = (byte*)outDest;
			for (uint i = 0; i < offset; i++) {
				dstBytes[i] = srcBytes[i];
			}

			// uintで一気に転送
			uint* dst = (uint*)((byte*)outDest + offset);
			uint* src = (uint*)((byte*)inSrc + offset);
			uint numOfUInt = (inNumOfBytes - offset) / sizeof(uint);
			for (uint i = 0; i < numOfUInt; i++) {
				dst[i] = src[i];
			}

			// 末尾の余り部分をbyteでちまちまコピー
			for (uint i = offset + numOfUInt * sizeof(uint); i < inNumOfBytes; i++) {
				dstBytes[i] = srcBytes[i];
			}
		}

		#endregion
	}
}
