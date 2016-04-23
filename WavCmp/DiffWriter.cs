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
using System.IO;
using System.Text;

namespace WavCmp
{
	/// <summary>
	/// 差分情報の書き出しを行うクラス
	/// </summary>
	class DiffWriter : IDisposable
	{
		/// <summary>ファイル書き出し用のオブジェクト</summary>
		private StreamWriter sw_;
		/// <summary>初回書き出しフラグ</summary>
		private bool firstOut_;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="path">書き出しファイルパス</param>
		public DiffWriter(string path)
		{
			firstOut_ = true;
			try {
				sw_ = new StreamWriter(path, false, Encoding.GetEncoding("shift_jis"));
			} catch (Exception) {   // 乱暴だがこれで問題になる状況はよほどの状況
				Close();
				throw;
			}

			InitializeDisposeFinalizePattern();
		}

		/// <summary>
		/// ファイルをクローズします。
		/// </summary>
		public void Close()
		{
			Dispose();
		}

		/// <summary>
		/// 差分を出力します。
		/// </summary>
		/// <param name="addr">差分アドレス</param>
		/// <param name="b1">出力データ1</param>
		/// <param name="b2">出力データ2</param>
		public void PrintDiff(long addr, byte b1, byte b2)
		{
			if (firstOut_){
				sw_.WriteLine("Addr,\tdata1,\tdata2");
			}
			sw_.WriteLine("0x" + addr.ToString("X8") + ",\t" + b1.ToString("X2") + ",\t" + b2.ToString("X2"));
			firstOut_ = false;
		}
		/// <summary>
		/// 差分を出力します。
		/// </summary>
		/// <param name="addr">差分アドレス</param>
		/// <param name="s1">出力データ1</param>
		/// <param name="s2">出力データ2</param>
		/// <param name="hex">16進数出力なら真</param>
		public void PrintDiff(long addr, short s1, short s2, bool hex)
		{
			if (firstOut_) {
				sw_.WriteLine("Addr,\tdata1,\tdata2");
			}
			if (hex) {
				sw_.WriteLine("0x" + addr.ToString("X8") + ",\t" + s1.ToString("X4") + ",\t" + s2.ToString("X4"));
			} else {
				sw_.WriteLine("0x" + addr.ToString("X8") + ",\t" + s1.ToString() + ",\t" + s2.ToString());
			}
			firstOut_ = false;
		}
		/// <summary>
		/// 差分を出力します。
		/// </summary>
		/// <param name="addr">差分アドレス</param>
		/// <param name="n1">出力データ1</param>
		/// <param name="n2">出力データ2</param>
		/// <param name="hex">16進数出力なら真</param>
		/// <param name="hex">16進数出力時の桁数</param>
		public void PrintDiff(long addr, int n1, int n2, bool hex, int align)
		{
			if (firstOut_) {
				sw_.WriteLine("Addr,\tdata1,\tdata2");
			}
			if (hex) {
				if (align == 6) {
					sw_.WriteLine("0x" + addr.ToString("X8") + ",\t" + n1.ToString("X6") + ",\t" + n2.ToString("X6"));
				} else {
					sw_.WriteLine("0x" + addr.ToString("X8") + ",\t" + n1.ToString("X8") + ",\t" + n2.ToString("X8"));
				}
			} else {
				sw_.WriteLine("0x" + addr.ToString("X8") + ",\t" + n1.ToString() + ",\t" + n2.ToString());
			}
			firstOut_ = false;
		}
		/// <summary>
		/// 差分を出力します。
		/// </summary>
		/// <param name="addr">差分アドレス</param>
		/// <param name="n1">出力データ1</param>
		/// <param name="n2">出力データ2</param>
		/// <param name="hex">16進数出力なら真</param>
		public void PrintDiff(long addr, float n1, float n2, bool hex)
		{
			if (firstOut_) {
				sw_.WriteLine("Addr,\tdata1,\tdata2");
			}
			if (hex) {
				byte[] arr1 = BitConverter.GetBytes(n1);
				byte[] arr2 = BitConverter.GetBytes(n2);
				sw_.Write("0x" + addr.ToString("X8") + ",\t");
				for (int i = 3; i >= 0; i--) {
					sw_.Write(arr1[i].ToString("X"));
				}
				sw_.Write(",\t");
				for (int i = 3; i >= 0; i--) {
					sw_.Write(arr2[i].ToString("X"));
				}
				sw_.WriteLine("");
			} else {
				sw_.WriteLine("0x" + addr.ToString("X8") + ",\t" + n1.ToString() + ",\t" + n2.ToString());
			}
			firstOut_ = false;
		}
		/// <summary>
		/// 結果の統計情報の出力
		/// </summary>
		/// <param name="count">相違点数</param>
		/// <param name="imax">最大の相違（整数）</param>
		public void PrintResult(long count, long imax)
		{
			sw_.WriteLine("--------------------------------");
			sw_.WriteLine("最大の相違（10進数）:" + imax.ToString());
			sw_.WriteLine("相違点数:" + count.ToString());
		}
		/// <summary>
		/// 結果の統計情報の出力
		/// </summary>
		/// <param name="count">相違点数</param>
		/// <param name="fmax">最大の相違（実数）</param>
		public void PrintResult(long count, float fmax)
		{
			sw_.WriteLine("--------------------------------");
			sw_.WriteLine("最大の相違（10進数）:" + fmax.ToString());
			sw_.WriteLine("相違点数:" + count.ToString());
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
		~DiffWriter()
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
				if (sw_ != null) {
					sw_.Close();
					sw_ = null;
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
			return sw_.GetHashCode();
		}
		/// <summary>
		/// 文字列表現を取得します。
		/// </summary>
		/// <returns>文字列表現</returns>
		public override string ToString()
		{
			return sw_.ToString();
		}

		#endregion

	}
}
