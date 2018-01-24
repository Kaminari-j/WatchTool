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
using System.Collections;

namespace WatchTool
{
	public class Downloader
	{
		protected bool _finishFlag = false;

		public static string GetContentsFromSrc(string url)
		{
			try
			{
				string strContent = null;

				var webRequest = WebRequest.Create(url);

				using (var response = webRequest.GetResponse())
				using (var content = response.GetResponseStream())
				using (var reader = new StreamReader(content))
				{
					strContent = reader.ReadToEnd();
				}

				return strContent;
			}
			catch (Exception ex)
			{
				Common.ShowExceptionMsgBox(ex);
				return null;
			}
		}

		public static void DownloadThread(List<FileData> files, IControlInterface controlerForm)
		{
			try
			{
				foreach (FileData data in files)
				{
					using (WebClient webClient = new WebClient())
					{
						// Download
						webClient.DownloadFile(data.URL, data.FileName);

						// read image from file, and delete tmp file?
					}

					// Set File Names to listbox
					controlerForm.DoAddListBoxValue(data.FileName);
					// Progressbar step
					controlerForm.DoPerformProgressBarStep();
				}

				// Reset Progress Bar
				controlerForm.ResetProgressBar();
			}
			catch (Exception ex)
			{
				Common.ShowExceptionMsgBox(ex);
			}
		}

		//protected void DownloadTwitterVideo(List<string> urllist, IControlInterface controlerForm)
		//{
		//	foreach (string url in urllist)
		//	{
		//		// ex) "https://video.twimg.com/ext_tw_video/951480266375098369/pr/pl/316x180/_tsyxIfYoocMkpYD.m3u8"
		//		string targetUrl = this.GetOriginalImageUrl(url);
		//		string fullName = MakeUniqueFileName(this._DownloadDIR
		//														+ @"\" + DateTime.Now.ToString("yyyyMMdd")
		//														+ "_" + this.SERVICE_NAME
		//														+ "_",
		//									  imgUrl,
		//									  MEDIATYPE.video
		//									 ).FullName;

		//		Process ffmpeg = new Process
		//		{
		//			StartInfo =
		//		{
		//			FileName = @".\..\..\FFMPEG\ffmpeg.exe",
		//			Arguments = String.Format(@"-i ""{0}"" -bsf:a aac_adtstoasc -vcodec copy -c copy -crf 50 {1}",
		//										targetUrl,
		//										fullName),
		//			UseShellExecute = false,
		//			RedirectStandardOutput = true,
		//			CreateNoWindow = true,
		//			WorkingDirectory = this._DownloadDIR
		//		}
		//		};

		//		ffmpeg.EnableRaisingEvents = true;
		//		ffmpeg.OutputDataReceived += (s, e) => Debug.WriteLine(e.Data);
		//		ffmpeg.ErrorDataReceived += (s, e) => Debug.WriteLine($@"Error: {e.Data}");
		//		ffmpeg.Start();
		//		ffmpeg.BeginOutputReadLine();
		//		ffmpeg.WaitForExit();
		//	}
		//}
	}
}
