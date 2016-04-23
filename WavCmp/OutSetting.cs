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
using System.Windows.Forms;

namespace WavCmp
{
	/// <summary>
	/// 比較設定
	/// </summary>
	public struct CompareSetting
	{
		/// <summary>バイト単位比較かサンプル単位比較か</summary>
		public bool sampleOrder;
		public bool hexOut;
		public bool streamOrigin;
	}

	public partial class OutSetting : Form
	{
		private CompareSetting m_Setting;

		public CompareSetting Setting
		{
			get { return m_Setting; }
			set { m_Setting = value; }
		}

		public OutSetting()
		{
			InitializeComponent();
		}

		private void OutSetting_Load(object sender, EventArgs e)
		{
			if (m_Setting.sampleOrder) {
				rBSample.Checked = true;
				rBByte.Checked = false;
			} else {
				rBSample.Checked = false;
				rBByte.Checked = true;
			}
			if (m_Setting.hexOut) {
				rB16.Checked = true;
				rB10.Checked = false;
			} else {
				rB16.Checked = false;
				rB10.Checked = true;
			}
			if (m_Setting.streamOrigin) {
				rBOriginStream.Checked = true;
				rBOriginFile.Checked = false;
			} else {
				rBOriginStream.Checked = false;
				rBOriginFile.Checked = true;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			m_Setting.sampleOrder = rBSample.Checked;
			m_Setting.hexOut = rB16.Checked;
			m_Setting.streamOrigin = rBOriginStream.Checked;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
