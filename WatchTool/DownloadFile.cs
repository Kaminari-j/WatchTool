using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace WatchTool
{
	public class DownloadFile
	{
		protected WatchTool.SERVICE SERVICE_NAME = SERVICE.NONE;
		protected WatchTool.MEDIATYPE MEDIA_TYPE = MEDIATYPE.NONE;
		protected String _grepKeyword { get; set; }
		private String _Content { get; set; }
		private String _url { get; set; }
		public List<string> _DownloadList { get; set; }
		protected IControlInterface frm;
		private bool _finishFlag = false;
		private string _DownloadDIR { get; set; }

		public DownloadFile(string url, SERVICE SVC, IControlInterface MainFrm)
		{
			this._url = SetTargetUrl(url);
			this.SERVICE_NAME = SVC;
			this.frm = MainFrm;
		}

		public bool StartDownload()
		{
			try
			{
				bool dl_result = false;

				if (string.IsNullOrEmpty(this._url))
				{
					return false;
				}

				if (this.GetContentsFromSrc(this._url) == true)
				{
					List<string> fileList = MakeFileList(this._Content);
					if (fileList.Count >= 1)
					{
						dl_result = DoDownloadFile(fileList, true);
					}
				}

				return dl_result;
			}
			catch (Exception ex)
			{
				ShowExceptionMsgBox(ex);
				return false;
			}
		}

		protected virtual string SetTargetUrl(string url)
		{
			return url;
		}

		protected bool GetContentsFromSrc(string url)
		{
			try
			{
				var webRequest = WebRequest.Create(url);

				using (var response = webRequest.GetResponse())
				using (var content = response.GetResponseStream())
				using (var reader = new StreamReader(content))
				{
					this._Content = reader.ReadToEnd();
				}

				return true;
			}
			catch (Exception ex)
			{
				ShowExceptionMsgBox(ex);
				return false;
			}
		}

		protected List<string> MakeFileList(string strWebContent)
		{
			try
			{
				if (String.IsNullOrEmpty(this._Content))
					return null;

				List<string> _filelist = GetFileListFromContent(strWebContent);

				if (_filelist.Count == 0)
					return null;
				else
					return _filelist;
			}
			catch (Exception ex)
			{
				ShowExceptionMsgBox(ex);
				return null;
			}
		}

		protected virtual List<string> GetFileListFromContent(string content)
		{
			throw new NotImplementedException();
		}

		protected bool DoDownloadFile(List<string> lstFiles)
		{
			// Download from web
			try
			{
				// SetProgressbarMax
				frm.DoSetProgressBarMaxValue(lstFiles.Count);

				foreach (string imgUrl in lstFiles)
				{
					string targetUrl = this.GetOriginalImageName(imgUrl);

					string fullName = "";
					fullName = MakeUniqueFileName(System.Environment.CurrentDirectory
													+ @"\" + DateTime.Now.ToString("yyyyMMdd")
													+ "_" + this.SERVICE_NAME
													+ "_"
													, imgUrl
													, MEDIATYPE.image
												).FullName;

					// Set File Names to listbox
					this.frm.DoAddListBoxValue(fullName);

					using (WebClient webClient = new WebClient())
					{
						// Download
						webClient.DownloadFile(targetUrl, fullName);

						// read image from file, and delete tmp file?
					}
					// Progressbar step
					frm.DoPerformProgressBarStep();
				}

				// Reset Progress Bar
				frm.DoResetProgressBar();
				return true;
			}
			catch (Exception ex)
			{
				ShowExceptionMsgBox(ex);
				return false;
			}
		}

		protected bool DoDownloadFile(List<string> lstFiles, bool isThread)
		{
			// Download from web
			try
			{
				this._DownloadList = lstFiles;
				// SetProgressbarMax
				frm.DoSetProgressBarMaxValue(this._DownloadList.Count);

				// Directory Initialize
				this._DownloadDIR = (string.IsNullOrEmpty(Properties.Settings.Default.DownloadDir)) ? Application.StartupPath + @"\Download" : Properties.Settings.Default.DownloadDir;
				DirectoryInfo di = new DirectoryInfo(_DownloadDIR);
				if (di.Exists == false)
				{
					di.Create();
				}

				//Thread th = new Thread(new ThreadStart(DownloadThread));
				Thread th = new Thread(() => DownloadThread());
				th.Start();

				return true;
			}
			catch (Exception ex)
			{
				ShowExceptionMsgBox(ex);
				return false;
			}
		}

		public void DownloadThread()
		{
			try
			{
				foreach (string imgUrl in this._DownloadList)
				{
					string targetUrl = this.GetOriginalImageName(imgUrl);

					string fullName = "";
					fullName = MakeUniqueFileName(this._DownloadDIR
													+ @"\" + DateTime.Now.ToString("yyyyMMdd")
													+ "_" + this.SERVICE_NAME
													+ "_"
													, imgUrl
													, MEDIATYPE.image
												).FullName;

					using (WebClient webClient = new WebClient())
					{
						// Download
						webClient.DownloadFile(targetUrl, fullName);

						// read image from file, and delete tmp file?
					}

					// Set File Names to listbox
					this.frm.DoAddListBoxValue(fullName);
					// Progressbar step
					frm.DoPerformProgressBarStep();
				}

				// Reset Progress Bar
				frm.DoResetProgressBar();
				this._finishFlag = true;
			}
			catch (Exception ex)
			{
				ShowExceptionMsgBox(ex);
			}
		}

		protected void DownloadTwitterVideo()
		{
			foreach (string imgUrl in this._DownloadList)
			{
				// ex) "https://video.twimg.com/ext_tw_video/951480266375098369/pr/pl/316x180/_tsyxIfYoocMkpYD.m3u8"
				string targetUrl = this.GetOriginalImageName(imgUrl);
				string fullName = MakeUniqueFileName(this._DownloadDIR
																+ @"\" + DateTime.Now.ToString("yyyyMMdd")
																+ "_" + this.SERVICE_NAME
																+ "_",
											  imgUrl,
											  MEDIATYPE.video
											 ).FullName;

				Process ffmpeg = new Process
				{
					StartInfo =
				{
					FileName = @".\..\..\FFMPEG\ffmpeg.exe",
					Arguments = String.Format(@"-i ""{0}"" -bsf:a aac_adtstoasc -vcodec copy -c copy -crf 50 {1}",
												targetUrl,
												fullName),
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					WorkingDirectory = this._DownloadDIR
				}
				};

				ffmpeg.EnableRaisingEvents = true;
				ffmpeg.OutputDataReceived += (s, e) => Debug.WriteLine(e.Data);
				ffmpeg.ErrorDataReceived += (s, e) => Debug.WriteLine($@"Error: {e.Data}");
				ffmpeg.Start();
				ffmpeg.BeginOutputReadLine();
				ffmpeg.WaitForExit();
			}
		}

		public FileInfo MakeUniqueFileName(string path, string imgUrl, MEDIATYPE _MediaType)
		{
			string dir = Path.GetDirectoryName(path);
			string fileName = Path.GetFileNameWithoutExtension(path);
			string fileExt = _MediaType == MEDIATYPE.image ? Path.GetExtension(imgUrl) : "mp4";

			for (int i = 1; ; ++i)
			{
				path = Path.Combine(dir, fileName + i + fileExt);

				if (!File.Exists(path))
					return new FileInfo(path);
			}
		}

		protected virtual string GetOriginalImageName(string imgUrl)
		{
			// orig をつけるかほかの方法

			return imgUrl;
		}

		protected virtual string GetOriginalMovieName(string imgUrl)
		{
			// orig をつけるかほかの方法

			return imgUrl;
		}

		protected void ShowExceptionMsgBox(Exception ex)
		{
			MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
		}

		protected void ShowErrorMsgBox(string msg)
		{
			MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}

	public class DownloadInstagram : DownloadFile
	{
		// https://www.instagram.com/p/BdcnRlSl4Yh
		public DownloadInstagram(String url, IControlInterface MainFrm) : base(url, SERVICE.instagram, MainFrm) { }

		protected override string SetTargetUrl(string url)
		{
			try
			{
				return System.Text.RegularExpressions.Regex.Matches(url, @"https\:\/\/www\.instagram\.com\/p\/[0-9a-zA-Z\-]+/")[0].ToString() + "?__a=1";
			}
			catch (Exception)
			{
				this.ShowErrorMsgBox(url + "\r\n이 주소는 아직(?)지원하지 않습니다!");
				return null;
			}
		}

		protected override List<string> GetFileListFromContent(string content)
		{
			List<string> tmpFileList = new List<string>();
			JObject jobj = JObject.Parse(content);

			string mediaType = jobj["graphql"]["shortcode_media"]["__typename"].ToString();

			if (mediaType == "GraphVideo")
			{
				this.MEDIA_TYPE = MEDIATYPE.video;
				tmpFileList.Add(jobj["graphql"]["shortcode_media"]["video_url"].ToString());
			}
			else if (mediaType == "GraphImage")
			{
				this.MEDIA_TYPE = MEDIATYPE.image;
				tmpFileList.Add(jobj["graphql"]["shortcode_media"]["display_url"].ToString());
			}
			else if (mediaType == "GraphSidecar")
			{
				foreach (JObject file in jobj["graphql"]["shortcode_media"]["edge_sidecar_to_children"]["edges"])
				{
					mediaType = file["node"]["__typename"].ToString();
					if (mediaType == "GraphVideo")
					{
						tmpFileList.Add(file["node"]["video_url"].ToString());
					}
					else if (mediaType == "GraphImage")
					{
						tmpFileList.Add(file["node"]["display_url"].ToString());
					}
				}
			}

			return tmpFileList;
		}
	}

	public class DownloadTwitter : DownloadFile
	{
		public DownloadTwitter(String url, IControlInterface MainFrm) : base(url, SERVICE.twitter, MainFrm)
		{
			this._grepKeyword = @"data-image-url=.*";
		}

		protected override List<string> GetFileListFromContent(string content)
		{
			this.MEDIA_TYPE = MEDIATYPE.image; // Twitterの動画ダウンロードは今後追加するため
			MatchCollection tmp = System.Text.RegularExpressions.Regex.Matches(content, this._grepKeyword);
			List<string> tmpFileList = new List<string>();
			JObject jobj = JObject.Parse(content);
			foreach (Match file in tmp)
			{
				tmpFileList.Add(file.ToString());
			}

			return tmpFileList;
		}

		protected override string GetOriginalImageName(string imgUrl)
		{
			// 末尾に :orig をつける
			// 引数 imgUrl の文字列の例 "data-image-url="https://pbs.twimg.com/media/DDp82xwUMAEDOpz.jpg""
			try
			{
				string[] spltRst = imgUrl.Split('\"');
				if (spltRst.Length == 3)
					return spltRst[1] + ":orig";
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + ex.StackTrace);

				return string.Empty;
			}

			return string.Empty;
		}

		protected override string GetOriginalMovieName(string imgUrl)
		{
			// orig をつけるかほかの方法

			return string.Empty;
		}
	}
}
