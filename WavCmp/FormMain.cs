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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PcmLib;

namespace WavCmp
{
	public partial class FormMain : Form
	{
		private enum CompType
		{
			Wavs,
			RawWav,
			Raws
		}

		/// <summary>
		/// 詳細比較方式の構造体
		/// </summary>
		private struct CompareInfo
		{
			/// <summary>比較設定</summary>
			public CompareSetting setting;
			/// <summary>比較方式</summary>
			public CompType type;
			/// <summary>RAWデータとして扱うファイルの番号</summary>
			public int rawFile;
			
		}

		/// <summary>詳細比較方式</summary>
		private CompareInfo m_Comp;

		public FormMain()
		{
			InitializeComponent();

			m_Comp = new CompareInfo();
		}

		#region フォームのイベントハンドラ

		/// <summary>
		/// フォーム起動時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormMain_Load(object sender, EventArgs e)
		{
			// 引数で受けた場合（優先）
			string[] cmd = System.Environment.GetCommandLineArgs();
			if (cmd.Length > 1) {
				textBoxFile1.Text = cmd[1];
			}
			if (cmd.Length > 2) {
				textBoxFile2.Text = cmd[2];
			}

			LoadSetting();
		}
		/// <summary>
		/// フォームを閉じる際の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveSetting();
		}
		/// <summary>
		/// ファイルドラッグ時処理。
		/// ファイル以外の場合は禁止します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormMain_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				string[] drags = (string[])e.Data.GetData(DataFormats.FileDrop);
				foreach (string d in drags) {
					if (!System.IO.File.Exists(d)) {
						return;
					}
				}
				e.Effect = DragDropEffects.Copy;
			}
		}
		/// <summary>
		/// ファイルドロップ時処理。
		/// 比較ファイルパスにファイルパスを設定します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormMain_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files.Length == 1) {
				if (textBoxFile1.Text == "") {
					textBoxFile1.Text = files[0];
				} else if (textBoxFile2.Text == "") {
					textBoxFile2.Text = files[0];
				} else {
					textBoxFile1.Text = files[0];
				}
			} else if (files.Length > 1) {
				textBoxFile1.Text = files[0];
				textBoxFile2.Text = files[1];
			}
		}

		#endregion

		#region ボタン等のコントロール関連のイベントハンドラ
		
		/// <summary>
		/// ファイル1の選択
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRef1_Click(object sender, EventArgs e)
		{
			DialogResult res = openFD.ShowDialog();
			if (res == DialogResult.OK) {
				textBoxFile1.Text = openFD.FileName;
			}
		}
		/// <summary>
		/// ファイル2の選択
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRef2_Click(object sender, EventArgs e)
		{
			DialogResult res = openFD.ShowDialog();
			if (res == DialogResult.OK) {
				textBoxFile2.Text = openFD.FileName;
			}
		}
		/// <summary>
		/// 差分書き出しファイルの選択
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOut_Click(object sender, EventArgs e)
		{
			DialogResult res = saveFD.ShowDialog();
			if (res == DialogResult.OK) {
				textBoxOut.Text = saveFD.FileName;
			}
		}
		/// <summary>
		/// 比較ボタンクリック
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCmp_Click(object sender, EventArgs e)
		{
			// 入力ファイルチェック
			if (textBoxFile1.Text == "" || textBoxFile2.Text == "") {
				MessageBox.Show("有効なファイル名を指定してください。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (!File.Exists(textBoxFile1.Text)) {
				MessageBox.Show("ファイル1は存在しません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (!File.Exists(textBoxFile2.Text)) {
				MessageBox.Show("ファイル2は存在しません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (!CheckFileType()) {
				return;
			}

			// 比較開始
			progressBarMain.Value = 0;
			ChangeBtnState(true);
			string[] args = { textBoxFile1.Text, textBoxFile2.Text, textBoxOut.Text };
			bgWorkerMain.RunWorkerAsync(args);
		}
		/// <summary>
		/// 設定ボタンクリック
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOption_Click(object sender, EventArgs e)
		{
			OutSetting dlg = new OutSetting();
			dlg.Setting = m_Comp.setting;
			dlg.ShowDialog();
			m_Comp.setting = dlg.Setting;
		}
		/// <summary>
		/// ファイル1のパス変更発生
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBoxFile1_TextChanged(object sender, EventArgs e)
		{
			ChangeCmpBtnState();
		}
		/// <summary>
		/// ファイル2のパス変更発生
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBoxFile2_TextChanged(object sender, EventArgs e)
		{
			ChangeCmpBtnState();
		}

		#endregion

		#region BackGroundWorkerのイベントハンドラ

		/// <summary>
		/// Workerスレッド内処理。比較処理を行います。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bgWorkerMain_DoWork(object sender, DoWorkEventArgs e)
		{
			DiffWriter sw = null;
			string[] args = (string[])e.Argument;

			int ret = 0;
			try {
				if (args[2] != "") {
					sw = new DiffWriter(args[2]);
				}

				if (m_Comp.type == CompType.Wavs) {
					ret = CompareWav((BackgroundWorker)sender, args, m_Comp, sw);
				} else if (m_Comp.type == CompType.RawWav) {
					// 現状ここには来ない
					ret = CompareWavRaw((BackgroundWorker)sender, args, m_Comp, sw);
				} else {
					ret = CompareRaw((BackgroundWorker)sender, args, m_Comp, sw);
				}
			} finally {
				if (sw != null) {
					sw.Close();
				}
			}

			e.Result = ret;
		}

		private void bgWorkerMain_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBarMain.Value = e.ProgressPercentage;
		}

		private void bgWorkerMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			int result = (int)e.Result;
			if (result == 0) {
				MessageBox.Show("完全に一致しました。");
			} else if (result == 1) {
				if (textBoxOut.Text != "") {
					DialogResult ret = MessageBox.Show("相違点が見つかりました。差分出力ファイルを開きますか？", "結果", MessageBoxButtons.YesNo);
					if (ret == DialogResult.Yes) {
						Process.Start(textBoxOut.Text);
					}
				} else {
					MessageBox.Show("相違点が見つかりました。");
				}
			} else if (result == -1) {
				MessageBox.Show("比較中にエラーが発生しました。");
			}
			ChangeBtnState(false);
		}

		#endregion

		#region その他雑多な下請け

		private void SaveSetting()
		{
			// 比較ファイルを覚えておく
			Properties.Settings.Default.File1 = textBoxFile1.Text;
			Properties.Settings.Default.File2 = textBoxFile2.Text;
			Properties.Settings.Default.FileOut = textBoxOut.Text;

			// 設定を覚えておく
			Properties.Settings.Default.SampleOrder = m_Comp.setting.sampleOrder;
			Properties.Settings.Default.HexOut = m_Comp.setting.hexOut;
			Properties.Settings.Default.StreamOrigin = m_Comp.setting.streamOrigin;

			// アプリケーションの設定を保存
			Properties.Settings.Default.Save();
		}

		private void LoadSetting()
		{
			// 前回のファイルを設定する
			if (textBoxFile1.Text == "") {
				textBoxFile1.Text = Properties.Settings.Default.File1;
			}
			if (textBoxFile2.Text == "") {
				textBoxFile2.Text = Properties.Settings.Default.File2;
			}
			textBoxOut.Text = Properties.Settings.Default.FileOut;

			m_Comp.setting.sampleOrder = Properties.Settings.Default.SampleOrder;
			m_Comp.setting.hexOut = Properties.Settings.Default.HexOut;
			m_Comp.setting.streamOrigin = Properties.Settings.Default.StreamOrigin;
		}

		private int CompareWav(BackgroundWorker worker, string[] files, CompareInfo ci, DiffWriter sw)
		{
			CompareCore cc = new CompareCore(sw);
			cc.HexOut = ci.setting.hexOut;
			int ret = 0;
			try {
				using (RiffReader rr1 = new RiffReader(files[0]))
				using (RiffReader rr2 = new RiffReader(files[1])) {
					if (!rr1.Parse()) {
						return -1;
					}
					if (!rr2.Parse()) {
						return -1;
					}
					if (!ci.setting.streamOrigin) {
						cc.Offset = rr1.StreamOffset;
					}

					// 読み込み終端。単位に注意
					long limit = 0;
					if (ci.setting.sampleOrder) {
						limit = (rr1.Samples > rr2.Samples) ? rr2.Samples : rr1.Samples;
					} else {
						limit = (rr1.Length > rr2.Length) ? rr2.Length : rr1.Length;
					}
					// 現在処理位置。単位に注意
					long read = 0;
					int prevProgress = 0;

					int BLOCK_LENGTH = 2 * 1024 * 1024;
					while (read < limit) {
						int size = BLOCK_LENGTH;
						if (read + BLOCK_LENGTH > limit) {
							size = (int)(limit - read);
						}

						if (ci.setting.sampleOrder) {
							int q = rr1.WaveFormat.BitsPerSample / 8;

							// まとめて読んで比較
							if (q == 1) {
								byte[] ba1 = rr1.Read8(size);
								byte[] ba2 = rr2.Read8(size);
								if (cc.CompareLoop(ba1, ba2, read)) {
									ret = 1;
								}
							} else if (q == 2) {
								short[] sa1 = rr1.Read16(size);
								short[] sa2 = rr2.Read16(size);
								if (cc.CompareLoop(sa1, sa2, read * q)) {
									ret = 1;
								}
							} else if (q == 3) {
								int[] sa1 = rr1.Read24(size);
								int[] sa2 = rr2.Read24(size);
								if (cc.CompareLoop(sa1, sa2, read * q, q)) {
									ret = 1;
								}
							} else if (q == 4) {
								if (rr1.WaveFormat.FormatTag == WaveFormatTag.IEEE_FLOAT) {
									float[] sa1 = rr1.Read32F(size);
									float[] sa2 = rr2.Read32F(size);
									if (cc.CompareLoop(sa1, sa2, read * q)) {
										ret = 1;
									}
								} else {
									int[] sa1 = rr1.Read32(size);
									int[] sa2 = rr2.Read32(size);
									if (cc.CompareLoop(sa1, sa2, read * q, q)) {
										ret = 1;
									}
								}
							} else {
								return -1;
							}
						} else {
							// まとめて読んで比較
							byte[] ba1 = rr1.Read8(size);
							byte[] ba2 = rr2.Read8(size);
							if (cc.CompareLoop(ba1, ba2, read)) {
								ret = 1;
							}
						}

						read += size;

						// 進捗を通知
						int progress = (int)(read * 100 / limit);
						if (progress > prevProgress) {
							prevProgress = progress;
							worker.ReportProgress(progress);
						}
					}
				}
			} catch {
				return -1;
			}
			if (sw != null && ret == 1) {
				if (cc.DiffType) {
					sw.PrintResult(cc.Count, cc.Max);
				} else {
					sw.PrintResult(cc.Count, cc.fMax);
				}
			}
			return ret;
		}

		private int CompareWavRaw(BackgroundWorker worker, string[] files, CompareInfo ci, DiffWriter sw)
		{
			int ret = 0;
			try {
			} catch {
				return -1;
			}
			return ret;
		}

		private int CompareRaw(BackgroundWorker worker, string[] files, CompareInfo ci, DiffWriter sw)
		{
			CompareCore cc = new CompareCore(sw);
			cc.HexOut = ci.setting.hexOut;

			int ret = 0;
			try {
				using (BinaryReader br1 = new BinaryReader(File.OpenRead(files[0])))	
				using (BinaryReader br2 = new BinaryReader(File.OpenRead(files[1]))) {
					br1.BaseStream.Seek(0, SeekOrigin.Begin);
					br2.BaseStream.Seek(0, SeekOrigin.Begin);

					long limit = (br1.BaseStream.Length > br2.BaseStream.Length) ? br2.BaseStream.Length : br1.BaseStream.Length;
					long read = 0;
					int prevProgress = 0;

					int BLOCK_LENGTH = 2 * 1024 * 1024;
					while (read < limit) {
						int size = BLOCK_LENGTH;
						if (read + BLOCK_LENGTH > limit) {
							size = (int)(limit - read);
						}

						// まとめて読んで
						byte[] ba1 = br1.ReadBytes(size);
						byte[] ba2 = br2.ReadBytes(size);

						// 比較
						if (cc.CompareLoop(ba1, ba2, read)) {
							ret = 1;
						}
						read += size;

						// 進捗を通知
						int progress = (int)(read * 100 / limit);
						if (progress > prevProgress) {
							prevProgress = progress;
							worker.ReportProgress(progress);
						}
					}
				}
			} catch {
				return -1;
			}
			if (sw != null && ret == 1) {
				sw.PrintResult(cc.Count, cc.Max);
			}
			return ret;
		}

		/// <summary>
		/// 比較ボタンの有効無効設定
		/// </summary>
		private void ChangeCmpBtnState()
		{
			if (File.Exists(textBoxFile1.Text) && File.Exists(textBoxFile2.Text)) {
				btnCmp.Enabled = true;
			} else {
				btnCmp.Enabled = false;
			}
		}
		/// <summary>
		/// ボタンの状態の設定
		/// </summary>
		/// <param name="running"></param>
		private void ChangeBtnState(bool running)
		{
			btnRef1.Enabled = !running;
			btnRef2.Enabled = !running;
			btnOut.Enabled = !running;
			btnOption.Enabled = !running;

			if (running) {
				textBoxFile1.ReadOnly = true;
				textBoxFile2.ReadOnly = true;
				textBoxOut.ReadOnly = true;
			} else {
				textBoxFile1.ReadOnly = false;
				textBoxFile2.ReadOnly = false;
				textBoxOut.ReadOnly = false;
			}
		}

		private bool CheckFileType()
		{
			try {
				using (RiffReader f1 = new RiffReader(textBoxFile1.Text))
				using (RiffReader f2 = new RiffReader(textBoxFile2.Text)) {
					bool b1 = f1.Parse();
					bool b2 = f2.Parse();

					if (b1 && b2) {
						if (f1.WaveFormat.Channels != f2.WaveFormat.Channels) {
							MessageBox.Show("チャンネル数が異なります。比較を中断します。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						} else if (f1.WaveFormat.FormatTag != f2.WaveFormat.FormatTag) {
							MessageBox.Show("フォーマットが異なります。比較を中断します。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						} else if (f1.WaveFormat.SamplesPerSecond != f2.WaveFormat.SamplesPerSecond) {
							MessageBox.Show("サンプリングレートが異なります。比較を中断します。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						} else if (f1.WaveFormat.BitsPerSample != f2.WaveFormat.BitsPerSample) {
							MessageBox.Show("量子化ビット数が異なります。比較を中断します。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						} else {
							m_Comp.type = CompType.Wavs;
						}
					} else if (!b1 && !b2) {
						MessageBox.Show("どちらもWAVファイルではありません。先頭からバイト単位で比較します。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						m_Comp.type = CompType.Raws;
					} else {
#if true
						m_Comp.type = CompType.Raws;
						if (b1) {
							MessageBox.Show("ファイル2はWAVファイルではありません。先頭からバイト単位で比較します。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							m_Comp.rawFile = 1;
						} else {
							MessageBox.Show("ファイル1はWAVファイルではありません。先頭からバイト単位で比較します。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							m_Comp.rawFile = 0;
						}
#else
						m_Comp.type = CompType.RawWav;
						if (b1) {
							MessageBox.Show("ファイル2はWAVファイルではありません。Rawストリームとみなして比較します。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							m_Comp.rawFile = 1;
						} else {
							MessageBox.Show("ファイル1はWAVファイルではありません。Rawストリームとみなして比較します。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							m_Comp.rawFile = 0;
						}
#endif
					}
					return true;
				}
			} catch (Exception) {
				MessageBox.Show("ファイルがオープンできません。比較を中断します。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}	
		}

		#endregion
	}
}
