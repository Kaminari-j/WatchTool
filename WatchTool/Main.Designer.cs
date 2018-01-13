namespace WatchTool
{
	partial class Main
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
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
			this.tbUrl = new System.Windows.Forms.TextBox();
			this.btnDownload = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.statusLabel_ServiceName = new System.Windows.Forms.ToolStripStatusLabel();
			this.listBox_Download = new System.Windows.Forms.ListBox();
			this.pbPictureSelected = new System.Windows.Forms.PictureBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.저장폴더설정DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnSortList = new System.Windows.Forms.Button();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbPictureSelected)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbUrl
			// 
			this.tbUrl.Location = new System.Drawing.Point(11, 33);
			this.tbUrl.Name = "tbUrl";
			this.tbUrl.Size = new System.Drawing.Size(842, 19);
			this.tbUrl.TabIndex = 0;
			this.tbUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUrl_KeyDown);
			// 
			// btnDownload
			// 
			this.btnDownload.Location = new System.Drawing.Point(859, 31);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.Size = new System.Drawing.Size(75, 23);
			this.btnDownload.TabIndex = 1;
			this.btnDownload.Text = "Download";
			this.btnDownload.UseVisualStyleBackColor = true;
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.statusLabel_ServiceName});
			this.statusStrip1.Location = new System.Drawing.Point(0, 407);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(947, 23);
			this.statusStrip1.SizingGrip = false;
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 17);
			// 
			// statusLabel_ServiceName
			// 
			this.statusLabel_ServiceName.Name = "statusLabel_ServiceName";
			this.statusLabel_ServiceName.Size = new System.Drawing.Size(160, 18);
			this.statusLabel_ServiceName.Text = "statusLabel_ServiceName";
			// 
			// listBox_Download
			// 
			this.listBox_Download.FormattingEnabled = true;
			this.listBox_Download.HorizontalScrollbar = true;
			this.listBox_Download.ItemHeight = 12;
			this.listBox_Download.Location = new System.Drawing.Point(12, 60);
			this.listBox_Download.Name = "listBox_Download";
			this.listBox_Download.Size = new System.Drawing.Size(546, 340);
			this.listBox_Download.TabIndex = 3;
			this.listBox_Download.SelectedIndexChanged += new System.EventHandler(this.listBox_Download_SelectedIndexChanged);
			this.listBox_Download.DoubleClick += new System.EventHandler(this.listBox_Download_DoubleClick);
			// 
			// pbPictureSelected
			// 
			this.pbPictureSelected.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.pbPictureSelected.BackgroundImage = global::WatchTool.Properties.Resources.background_img;
			this.pbPictureSelected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pbPictureSelected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbPictureSelected.Location = new System.Drawing.Point(596, 60);
			this.pbPictureSelected.Name = "pbPictureSelected";
			this.pbPictureSelected.Size = new System.Drawing.Size(339, 339);
			this.pbPictureSelected.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbPictureSelected.TabIndex = 4;
			this.pbPictureSelected.TabStop = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(947, 26);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.저장폴더설정DToolStripMenuItem,
            this.exitXToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(87, 22);
			this.optionsToolStripMenuItem.Text = "Options (&O)";
			// 
			// 저장폴더설정DToolStripMenuItem
			// 
			this.저장폴더설정DToolStripMenuItem.Name = "저장폴더설정DToolStripMenuItem";
			this.저장폴더설정DToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.저장폴더설정DToolStripMenuItem.Text = "저장폴더 설정 (&D)";
			this.저장폴더설정DToolStripMenuItem.Click += new System.EventHandler(this.SetDownloadDirToolStripMenuItem_Click);
			// 
			// exitXToolStripMenuItem
			// 
			this.exitXToolStripMenuItem.Name = "exitXToolStripMenuItem";
			this.exitXToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.exitXToolStripMenuItem.Text = "Exit (&X)";
			this.exitXToolStripMenuItem.Click += new System.EventHandler(this.exitXToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(54, 22);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// btnSortList
			// 
			this.btnSortList.Location = new System.Drawing.Point(564, 60);
			this.btnSortList.Name = "btnSortList";
			this.btnSortList.Size = new System.Drawing.Size(26, 339);
			this.btnSortList.TabIndex = 6;
			this.btnSortList.Text = "정렬";
			this.btnSortList.UseVisualStyleBackColor = true;
			this.btnSortList.Click += new System.EventHandler(this.btnSortList_Click);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(947, 430);
			this.Controls.Add(this.btnSortList);
			this.Controls.Add(this.pbPictureSelected);
			this.Controls.Add(this.listBox_Download);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.btnDownload);
			this.Controls.Add(this.tbUrl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.Name = "Main";
			this.Text = "sh_downloader";
			this.Load += new System.EventHandler(this.Main_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbPictureSelected)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbUrl;
		private System.Windows.Forms.Button btnDownload;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel_ServiceName;
		public System.Windows.Forms.ListBox listBox_Download;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.PictureBox pbPictureSelected;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 저장폴더설정DToolStripMenuItem;
		private System.Windows.Forms.Button btnSortList;
		private System.Windows.Forms.ToolStripMenuItem exitXToolStripMenuItem;
	}
}

