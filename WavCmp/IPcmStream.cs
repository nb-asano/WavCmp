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

namespace PcmLib
{
	/// <summary>
	/// WAVEFORMATEXTENSIBLEを考慮したWaveFormat構造体
	/// </summary>
	public struct WaveFormatEx
	{
		public short FormatTag;
		public short Channels;
		public int SamplesPerSecond;
		public int AverageBytesPerSecond;
		public short BlockAlign;
		public short BitsPerSample;
		/// <summary>WAVEFORMATEXTENSIBLEの有無</summary>
		public bool Extensible;
		/// <summary>WAVEFORMATEXTENSIBLEのSamples union</summary>
		public short Samples;
		/// <summary>WAVEFORMATEXTENSIBLEのチャンネルアサイン</summary>
		public uint ChannelMask;
		/// <summary>WAVEFORMATEXTENSIBLEのGUID</summary>
		public byte[] SubFormat;
	}

	/// <summary>
	/// WaveFormatTagに入りうるPCMの設定値
	/// </summary>
	public static class WaveFormatTag
	{
		public const int PCM = 1;
		public const int IEEE_FLOAT = 3;
		public const int ALAW = 6;
		public const int MULAW = 7;
		public const int EXTENSIBLE = -2;
	}

	/// <summary>
	/// mmreg.hで定義されているチャンネルアサインの値
	/// </summary>
	public static class WaveFormatChannelAssign
	{
		public const uint FRONT_LEFT = 0x1;
		public const uint FRONT_RIGHT = 0x2;
		public const uint FRONT_CENTER = 0x4;
		public const uint LOW_FREQUENCY = 0x8;
		public const uint BACK_LEFT = 0x10;
		public const uint BACK_RIGHT = 0x20;
		public const uint FRONT_LEFT_OF_CENTER = 0x40;
		public const uint FRONT_RIGHT_OF_CENTER = 0x80;
		public const uint BACK_CENTER = 0x100;
		public const uint SIDE_LEFT = 0x200;
		public const uint SIDE_RIGHT = 0x400;
		public const uint TOP_CENTER = 0x800;
		public const uint TOP_FRONT_LEFT = 0x1000;
		public const uint TOP_FRONT_CENTER = 0x2000;
		public const uint TOP_FRONT_RIGHT = 0x4000;
		public const uint TOP_BACK_LEFT = 0x8000;
		public const uint TOP_BACK_CENTER = 0x10000;
		public const uint TOP_BACK_RIGHT = 0x20000;
		public const uint RESERVED = 0x80000000;
	}

	/// <summary>
	/// PCMデータ入出力インターフェース
	/// </summary>
	public interface IPcmStream
	{
		/// <summary>ファイルのパス</summary>
		string FilePath { get; }
		/// <summary>ヘッダー情報</summary>
		WaveFormatEx WaveFormat { get; }
		/// <summary>ストリームの長さ</summary>
		long Length { get; }
		/// <summary>ストリームの読み出し位置</summary>
		long CurrentPos { get; }
		/// <summary>ストリームの開始バイトオフセット</summary>
		long StreamOffset { get; }

		/// <summary>
		/// ファイルの解析
		/// </summary>
		/// <returns>解析に成功すれば真</returns>
		bool Parse();
		/// <summary>
		/// ストリームデータの読み込み。要求したサイズ分のデータが読みだされるとは限りません。（例えばストリーム終端）
		/// </summary>
		/// <param name="n">読み取るバイトサイズ</param>
		/// <returns>読み取ったストリームデータ</returns>
		byte[] Read8(int n);
		/// <summary>
		/// 2byte単位でのストリームデータの読み込み。要求したサイズ分のデータが読みだされるとは限りません。（例えばストリーム終端）
		/// </summary>
		/// <param name="n">読み取るサイズ</param>
		/// <returns>読み取ったストリームデータ</returns>
		short[] Read16(int n);
		/// <summary>
		/// 3byte単位でのストリームデータの読み込み。要求したサイズ分のデータが読みだされるとは限りません。（例えばストリーム終端）
		/// </summary>
		/// <param name="n">読み取るサイズ</param>
		/// <returns>読み取ったストリームデータ</returns>
		int[] Read24(int n);
		/// <summary>
		/// 4byte単位でのストリームデータの読み込み。要求したサイズ分のデータが読みだされるとは限りません。（例えばストリーム終端）
		/// </summary>
		/// <param name="n">読み取るサイズ</param>
		/// <returns>読み取ったストリームデータ</returns>
		int[] Read32(int n);
		/// <summary>
		/// 4byte（float）単位でのストリームデータの読み込み。要求したサイズ分のデータが読みだされるとは限りません。（例えばストリーム終端）
		/// </summary>
		/// <param name="n">読み取るサイズ</param>
		/// <returns>読み取ったストリームデータ</returns>
		float[] Read32F(int n);
		/// <summary>
		/// ストリーム読み込み位置の変更
		/// </summary>
		/// <param name="n">移動バイトサイズ</param>
		/// <param name="origin">移動起点</param>
		/// <returns>移動後の位置。移動失敗時は負の数。</returns>
		long SeekStream(long n, SeekOrigin origin);
		/// <summary>
		/// ファイルのクローズ
		/// </summary>
		void Close();
	}
}
