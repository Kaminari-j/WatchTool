using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WatchToolLib;

namespace WatchTool
{
	delegate void DoAddListBoxValueCallback(string TextValue);
	delegate void DoPerformProgressBarStepCallback();
	delegate void DoResetProgressBarCallback();


	public partial class Main : Form, IControlInterface
	{
		public static string _DOWNLOAD_DIR = Application.StartupPath + @"\Download";

		public Main()
		{
			InitializeComponent();

			// initialize controls
			this.statusLabel_ServiceName.Text = "";
			DirectoryInfo di = new DirectoryInfo(_DOWNLOAD_DIR);
			if (di.Exists == false)
			{
				di.Create();
			}

		}

		#region -- Methods --

		private WatchToolLib.SERVICE GetServiceName(string url)
		{
			if (url.Contains("twitter.com"))
				return WatchToolLib.SERVICE.twitter;
			if (url.Contains("instagram.com"))
				return WatchToolLib.SERVICE.instagram;
			else
				return WatchToolLib.SERVICE.NONE;
		}

		private void onButtonclick()
		{
			string url = tbUrl.Text;
			SERVICE svc = GetServiceName(url);
			this.statusLabel_ServiceName.Text = svc.ToString();

			switch (svc)
			{
				case SERVICE.twitter:
					DownloadTwitter dt = new DownloadTwitter(this as IControlInterface, Properties.Settings.Default.DownloadDir);
					dt.StartDownload(url);
					break;
				case SERVICE.instagram:
					DownloadInstagram di = new DownloadInstagram(this as IControlInterface, Properties.Settings.Default.DownloadDir);
					di.StartDownload(url);
					break;
				case SERVICE.NONE:
				default:
					MessageBox.Show("해당 서비스는 지원되지 않습니다.");
					break;
			}
		}

		#region IContrlInterface
		public void DoAddListBoxValue(string TextValue)
		{
			if (this.listBox_Download.InvokeRequired)
			{
				DoAddListBoxValueCallback d = new DoAddListBoxValueCallback(DoAddListBoxValue);
				this.Invoke(d, new object[] { TextValue });
			}
			else
			{
				this.listBox_Download.Items.Add(TextValue);
			}
		}

		public void DoPerformProgressBarStep()
		{
			if (this.statusStrip1.InvokeRequired)
			{
				DoPerformProgressBarStepCallback d = new DoPerformProgressBarStepCallback(DoPerformProgressBarStep);
				this.Invoke(d, new object[] { });
			}
			else
			{
				this.toolStripProgressBar1.PerformStep();

				if (this.toolStripProgressBar1.Value == this.toolStripProgressBar1.Maximum)
				{
					MessageBox.Show("다운로드 완료!");
				}
			}
		}

		public void SetProgressBarMaxValue(int MaxValue)
		{
			this.toolStripProgressBar1.Maximum = MaxValue;
			this.toolStripProgressBar1.Step = 1;
		}

		public void DoSetListBoxData(string[] Data)
		{
			this.listBox_Download.DataSource = Data;
		}

		public void DoResetListBoxData()
		{
			string _DownloadDIR = (string.IsNullOrEmpty(Properties.Settings.Default.DownloadDir)) ? _DOWNLOAD_DIR : Properties.Settings.Default.DownloadDir;
			string[] _strFiles = Directory.GetFiles(_DownloadDIR);

			ArrayList downloadFiles = new ArrayList();
			foreach (string file in _strFiles)
			{
				downloadFiles.Add(file);
			}
			downloadFiles.Sort();

			this.listBox_Download.Items.Clear();

			foreach (string file in downloadFiles)
			{
				this.listBox_Download.Items.Add(file);
			}
		}

		public void ResetProgressBar()
		{
			if (this.statusStrip1.InvokeRequired)
			{
				DoResetProgressBarCallback d = new DoResetProgressBarCallback(ResetProgressBar);
				this.Invoke(d, new object[] { });
			}
			else
			{
				this.toolStripProgressBar1.Value = 0;
			}
		}
		#endregion

		#endregion

		#region -- HandleEvent --

		private void Main_Load(object sender, EventArgs e)
		{
			this.DoResetListBoxData();
		}

		private void listBox_Download_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ListBox)sender).SelectedItem != null)
			{
				if (((ListBox)sender).SelectedItem.ToString().Contains("mp4"))
				{ return; }
				else
				{
					this.pbPictureSelected.BackgroundImage = Image.FromFile(((ListBox)sender).SelectedItem.ToString());
				}
			}
		}
		private void tbUrl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				this.onButtonclick();
			}
		}

		private void btnDownload_Click(object sender, EventArgs e)
		{
			this.onButtonclick();
		}

		private void btnSortList_Click(object sender, EventArgs e)
		{
			this.DoResetListBoxData();
		}

		private void listBox_Download_DoubleClick(object sender, EventArgs e)
		{
			if (((ListBox)sender).SelectedItem != null)
			{
				Process.Start(((ListBox)sender).SelectedItem.ToString());
			}
		}

		#region -- Menu Strip
		private void SetDownloadDirToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.SelectedPath = (string.IsNullOrEmpty(Properties.Settings.Default.DownloadDir)) ? _DOWNLOAD_DIR : Properties.Settings.Default.DownloadDir;
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				Properties.Settings.Default.DownloadDir = fbd.SelectedPath;
				Properties.Settings.Default.Save();
				this.DoResetListBoxData();
			}
		}

		private void exitXToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		#endregion

		#endregion
	}
}
