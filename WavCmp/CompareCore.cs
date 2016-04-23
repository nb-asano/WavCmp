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

namespace WavCmp
{
	class CompareCore
	{
		/// <summary>ファイル出力オブジェクト</summary>
		private DiffWriter dw_;
		/// <summary>相違点の個数</summary>
		private long count_;
		/// <summary>最大の相違（整数）</summary>
		private long max_;
		/// <summary>最大の相違（浮動小数点）</summary>
		private float fmax_;
		/// <summary>16進数出力</summary>
		private bool hexOut_;
		/// <summary>アドレスオフセット</summary>
		private long offset_;
		/// <summary>相違点の形式</summary>
		private bool diffType_;

		/// <summary>相違点の個数</summary>
		public long Count
		{
			get { return count_; }
		}
		/// <summary>最大の相違（整数）</summary>
		public long Max
		{
			get { return max_; }
		}
		/// <summary>最大の相違（整数）</summary>
		public float fMax
		{
			get { return fmax_; }
		}
		/// <summary>16進数で出力するかどうか</summary>
		public bool HexOut
		{
			get { return hexOut_; }
			set { hexOut_ = value; }
		}
		/// <summary>アドレスのオフセット</summary>
		public long Offset
		{
			get { return offset_; }
			set { offset_ = value; }
		}
		/// <summary>相違点の型。真なら整数</summary>
		public bool DiffType
		{
			get { return diffType_; }
		}

		public CompareCore(DiffWriter dw)
		{
			dw_ = dw;
			count_ = 0;
			max_ = 0;
			hexOut_ = false;
			offset_ = 0;
			diffType_ = true;
		}

		/// <summary>
		/// 比較処理（バイト配列用）
		/// </summary>
		/// <param name="arr1">比較対象1</param>
		/// <param name="arr2">比較対象2</param>
		/// <param name="pos">比較対象の開始アドレス</param>
		/// <returns>差分があれば真</returns>
		public bool CompareLoop(byte[] arr1, byte[] arr2, long pos)
		{
			bool ret = false;

			for (int i = 0; i < arr1.Length; i++) {
				if (arr1[i] != arr2[i]) {
					count_++;
					int diff = (arr1[i] > arr2[i]) ? (arr1[i] - arr2[i]) : (arr2[i] - arr1[i]);
					if (diff > max_) {
						max_ = diff;
					}
					if (dw_ != null) {
						dw_.PrintDiff(offset_ + pos + i, arr1[i], arr2[i]);
					}
					ret = true;
				}
			}

			return ret;
		}
		/// <summary>
		/// 比較処理（short配列用）
		/// </summary>
		/// <param name="arr1">比較対象1</param>
		/// <param name="arr2">比較対象2</param>
		/// <param name="pos">比較対象の開始アドレス</param>
		/// <returns>差分があれば真</returns>
		public bool CompareLoop(short[] arr1, short[] arr2, long pos)
		{
			bool ret = false;

			for (int i = 0; i < arr1.Length; i++) {
				if (arr1[i] != arr2[i]) {
					count_++;
					int diff = (arr1[i] > arr2[i]) ? (arr1[i] - arr2[i]) : (arr2[i] - arr1[i]);
					if (diff > max_) {
						max_ = diff;
					}
					if (dw_ != null) {
						dw_.PrintDiff(offset_ + pos + i*2, arr1[i], arr2[i], hexOut_);
					}
					ret = true;
				}
			}

			return ret;
		}
		/// <summary>
		/// 比較処理（int配列用）
		/// </summary>
		/// <param name="arr1">比較対象1</param>
		/// <param name="arr2">比較対象2</param>
		/// <param name="pos">比較対象の開始アドレス</param>
		/// <param name="align">比較対象の実バイト数</param>
		/// <returns>差分があれば真</returns>
		public bool CompareLoop(int[] arr1, int[] arr2, long pos, int align)
		{
			bool ret = false;

			for (int i = 0; i < arr1.Length; i++) {
				if (arr1[i] != arr2[i]) {
					count_++;
					int diff = (arr1[i] > arr2[i]) ? (arr1[i] - arr2[i]) : (arr2[i] - arr1[i]);
					if (diff > max_) {
						max_ = diff;
					}
					if (dw_ != null) {
						dw_.PrintDiff(offset_ + pos + i * align, arr1[i], arr2[i], hexOut_, align*2);
					}
					ret = true;
				}
			}

			return ret;
		}
		/// <summary>
		/// 比較処理（float配列用）
		/// </summary>
		/// <param name="arr1">比較対象1</param>
		/// <param name="arr2">比較対象2</param>
		/// <param name="pos">比較対象の開始アドレス</param>
		/// <returns>差分があれば真</returns>
		public bool CompareLoop(float[] arr1, float[] arr2, long pos)
		{
			diffType_ = false;
			bool ret = false;

			for (int i = 0; i < arr1.Length; i++) {
				if (arr1[i] != arr2[i]) {
					count_++;
					float diff = (arr1[i] > arr2[i]) ? (arr1[i] - arr2[i]) : (arr2[i] - arr1[i]);
					if (diff > fmax_) {
						fmax_ = diff;
					}
					if (dw_ != null) {
						dw_.PrintDiff(offset_ + pos + i * 4, arr1[i], arr2[i], hexOut_);
					}
					ret = true;
				}
			}

			return ret;
		}
	}
}
