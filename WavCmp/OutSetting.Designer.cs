namespace WavCmp
{
	partial class OutSetting
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rBSample = new System.Windows.Forms.RadioButton();
			this.rBByte = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rB10 = new System.Windows.Forms.RadioButton();
			this.rB16 = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rBOriginStream = new System.Windows.Forms.RadioButton();
			this.rBOriginFile = new System.Windows.Forms.RadioButton();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rBSample);
			this.groupBox1.Controls.Add(this.rBByte);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(194, 49);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "比較単位";
			// 
			// rBSample
			// 
			this.rBSample.AutoSize = true;
			this.rBSample.Location = new System.Drawing.Point(88, 19);
			this.rBSample.Name = "rBSample";
			this.rBSample.Size = new System.Drawing.Size(85, 16);
			this.rBSample.TabIndex = 1;
			this.rBSample.Text = "サンプル単位";
			this.rBSample.UseVisualStyleBackColor = true;
			// 
			// rBByte
			// 
			this.rBByte.AutoSize = true;
			this.rBByte.Checked = true;
			this.rBByte.Location = new System.Drawing.Point(7, 19);
			this.rBByte.Name = "rBByte";
			this.rBByte.Size = new System.Drawing.Size(74, 16);
			this.rBByte.TabIndex = 0;
			this.rBByte.TabStop = true;
			this.rBByte.Text = "バイト単位";
			this.rBByte.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rB16);
			this.groupBox2.Controls.Add(this.rB10);
			this.groupBox2.Location = new System.Drawing.Point(12, 121);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(194, 49);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "差分出力";
			// 
			// rB10
			// 
			this.rB10.AutoSize = true;
			this.rB10.Checked = true;
			this.rB10.Location = new System.Drawing.Point(7, 19);
			this.rB10.Name = "rB10";
			this.rB10.Size = new System.Drawing.Size(59, 16);
			this.rB10.TabIndex = 0;
			this.rB10.TabStop = true;
			this.rB10.Text = "10進数";
			this.rB10.UseVisualStyleBackColor = true;
			// 
			// rB16
			// 
			this.rB16.AutoSize = true;
			this.rB16.Location = new System.Drawing.Point(72, 19);
			this.rB16.Name = "rB16";
			this.rB16.Size = new System.Drawing.Size(59, 16);
			this.rB16.TabIndex = 1;
			this.rB16.Text = "16進数";
			this.rB16.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.rBOriginFile);
			this.groupBox3.Controls.Add(this.rBOriginStream);
			this.groupBox3.Location = new System.Drawing.Point(12, 67);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(194, 48);
			this.groupBox3.TabIndex = 14;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "オフセット";
			// 
			// rBOriginStream
			// 
			this.rBOriginStream.AutoSize = true;
			this.rBOriginStream.Checked = true;
			this.rBOriginStream.Location = new System.Drawing.Point(7, 19);
			this.rBOriginStream.Name = "rBOriginStream";
			this.rBOriginStream.Size = new System.Drawing.Size(91, 16);
			this.rBOriginStream.TabIndex = 0;
			this.rBOriginStream.TabStop = true;
			this.rBOriginStream.Text = "ストリーム起点";
			this.rBOriginStream.UseVisualStyleBackColor = true;
			// 
			// rBOriginFile
			// 
			this.rBOriginFile.AutoSize = true;
			this.rBOriginFile.Location = new System.Drawing.Point(104, 19);
			this.rBOriginFile.Name = "rBOriginFile";
			this.rBOriginFile.Size = new System.Drawing.Size(81, 16);
			this.rBOriginFile.TabIndex = 1;
			this.rBOriginFile.Text = "ファイル起点";
			this.rBOriginFile.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(51, 176);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 15;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(132, 176);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 16;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// OutSetting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(219, 209);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OutSetting";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "出力設定";
			this.Load += new System.EventHandler(this.OutSetting_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rBSample;
		private System.Windows.Forms.RadioButton rBByte;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rB10;
		private System.Windows.Forms.RadioButton rB16;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton rBOriginStream;
		private System.Windows.Forms.RadioButton rBOriginFile;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}