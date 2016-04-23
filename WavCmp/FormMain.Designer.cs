namespace WavCmp
{
	partial class FormMain
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.btnRef1 = new System.Windows.Forms.Button();
			this.textBoxFile1 = new System.Windows.Forms.TextBox();
			this.btnRef2 = new System.Windows.Forms.Button();
			this.btnCmp = new System.Windows.Forms.Button();
			this.textBoxFile2 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.openFD = new System.Windows.Forms.OpenFileDialog();
			this.bgWorkerMain = new System.ComponentModel.BackgroundWorker();
			this.btnOut = new System.Windows.Forms.Button();
			this.textBoxOut = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.saveFD = new System.Windows.Forms.SaveFileDialog();
			this.progressBarMain = new System.Windows.Forms.ProgressBar();
			this.btnOption = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnRef1
			// 
			this.btnRef1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRef1.Location = new System.Drawing.Point(340, 13);
			this.btnRef1.Name = "btnRef1";
			this.btnRef1.Size = new System.Drawing.Size(32, 23);
			this.btnRef1.TabIndex = 0;
			this.btnRef1.Text = "...";
			this.btnRef1.UseVisualStyleBackColor = true;
			this.btnRef1.Click += new System.EventHandler(this.btnRef1_Click);
			// 
			// textBoxFile1
			// 
			this.textBoxFile1.Location = new System.Drawing.Point(71, 15);
			this.textBoxFile1.Name = "textBoxFile1";
			this.textBoxFile1.Size = new System.Drawing.Size(263, 19);
			this.textBoxFile1.TabIndex = 1;
			this.textBoxFile1.TextChanged += new System.EventHandler(this.textBoxFile1_TextChanged);
			// 
			// btnRef2
			// 
			this.btnRef2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRef2.Location = new System.Drawing.Point(340, 42);
			this.btnRef2.Name = "btnRef2";
			this.btnRef2.Size = new System.Drawing.Size(32, 23);
			this.btnRef2.TabIndex = 2;
			this.btnRef2.Text = "...";
			this.btnRef2.UseVisualStyleBackColor = true;
			this.btnRef2.Click += new System.EventHandler(this.btnRef2_Click);
			// 
			// btnCmp
			// 
			this.btnCmp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCmp.Enabled = false;
			this.btnCmp.Location = new System.Drawing.Point(297, 100);
			this.btnCmp.Name = "btnCmp";
			this.btnCmp.Size = new System.Drawing.Size(75, 23);
			this.btnCmp.TabIndex = 3;
			this.btnCmp.Text = "比較";
			this.btnCmp.UseVisualStyleBackColor = true;
			this.btnCmp.Click += new System.EventHandler(this.btnCmp_Click);
			// 
			// textBoxFile2
			// 
			this.textBoxFile2.Location = new System.Drawing.Point(71, 44);
			this.textBoxFile2.Name = "textBoxFile2";
			this.textBoxFile2.Size = new System.Drawing.Size(263, 19);
			this.textBoxFile2.TabIndex = 4;
			this.textBoxFile2.TextChanged += new System.EventHandler(this.textBoxFile2_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 12);
			this.label1.TabIndex = 5;
			this.label1.Text = "ファイル1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 12);
			this.label2.TabIndex = 6;
			this.label2.Text = "ファイル2";
			// 
			// bgWorkerMain
			// 
			this.bgWorkerMain.WorkerReportsProgress = true;
			this.bgWorkerMain.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerMain_DoWork);
			this.bgWorkerMain.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkerMain_ProgressChanged);
			this.bgWorkerMain.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerMain_RunWorkerCompleted);
			// 
			// btnOut
			// 
			this.btnOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOut.Location = new System.Drawing.Point(340, 71);
			this.btnOut.Name = "btnOut";
			this.btnOut.Size = new System.Drawing.Size(32, 23);
			this.btnOut.TabIndex = 12;
			this.btnOut.Text = "...";
			this.btnOut.UseVisualStyleBackColor = true;
			this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
			// 
			// textBoxOut
			// 
			this.textBoxOut.Location = new System.Drawing.Point(71, 73);
			this.textBoxOut.Name = "textBoxOut";
			this.textBoxOut.Size = new System.Drawing.Size(263, 19);
			this.textBoxOut.TabIndex = 13;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 76);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 12);
			this.label3.TabIndex = 14;
			this.label3.Text = "差分出力";
			// 
			// saveFD
			// 
			this.saveFD.DefaultExt = "txt";
			this.saveFD.Filter = "テキストファイル|*.txt|すべてのファイル|*.*";
			this.saveFD.Title = "差分情報の出力先";
			// 
			// progressBarMain
			// 
			this.progressBarMain.Location = new System.Drawing.Point(14, 100);
			this.progressBarMain.Name = "progressBarMain";
			this.progressBarMain.Size = new System.Drawing.Size(196, 23);
			this.progressBarMain.TabIndex = 15;
			// 
			// btnOption
			// 
			this.btnOption.Location = new System.Drawing.Point(216, 100);
			this.btnOption.Name = "btnOption";
			this.btnOption.Size = new System.Drawing.Size(75, 23);
			this.btnOption.TabIndex = 16;
			this.btnOption.Text = "設定";
			this.btnOption.UseVisualStyleBackColor = true;
			this.btnOption.Click += new System.EventHandler(this.btnOption_Click);
			// 
			// FormMain
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 136);
			this.Controls.Add(this.btnOption);
			this.Controls.Add(this.progressBarMain);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBoxOut);
			this.Controls.Add(this.btnOut);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxFile2);
			this.Controls.Add(this.btnCmp);
			this.Controls.Add(this.btnRef2);
			this.Controls.Add(this.textBoxFile1);
			this.Controls.Add(this.btnRef1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormMain";
			this.Text = "WavCmp";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnRef1;
		private System.Windows.Forms.TextBox textBoxFile1;
		private System.Windows.Forms.Button btnRef2;
		private System.Windows.Forms.Button btnCmp;
		private System.Windows.Forms.TextBox textBoxFile2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.OpenFileDialog openFD;
		private System.ComponentModel.BackgroundWorker bgWorkerMain;
		private System.Windows.Forms.Button btnOut;
		private System.Windows.Forms.TextBox textBoxOut;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.SaveFileDialog saveFD;
		private System.Windows.Forms.ProgressBar progressBarMain;
		private System.Windows.Forms.Button btnOption;
	}
}

