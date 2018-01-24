using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace WatchTool
{
	public class ServiceDownload
	{
		protected IControlInterface _ControlerForm;

		protected WatchTool.SERVICE SERVICE_NAME = SERVICE.NONE;
		protected WatchTool.MEDIATYPE MEDIA_TYPE = MEDIATYPE.NONE;

		private string _DownloadDIR { get; set; }

		public ServiceDownload(SERVICE serviceName, IControlInterface controlerForm)
		{
			this.SERVICE_NAME = serviceName;
			this._ControlerForm = controlerForm;

			DoInitialize();
		}

		public void DoInitialize()
		{
			try
			{
				// Directory Initialize
				this._DownloadDIR = (string.IsNullOrEmpty(Properties.Settings.Default.DownloadDir)) ? Application.StartupPath + @"\Download" : Properties.Settings.Default.DownloadDir;
				DirectoryInfo di = new DirectoryInfo(_DownloadDIR);
				if (di.Exists == false)
				{
					di.Create();
				}
			}
			catch (Exception ex)
			{
				Common.ShowExceptionMsgBox(ex);
			}
		}

		private List<FileData> PrefareDownload(string url)
		{
			if (string.IsNullOrEmpty(url))
				throw new ArgumentNullException();

			try
			{
				url = SetTargetUrl(url);
				string content = Downloader.GetContentsFromSrc(url);

				List<string> targetList = MakeUrlListFromContent(content);

				List<FileData> files = new List<FileData>();
				string mediaURL = null;
				string uploadDate = null;
				string fileName = null;

				foreach (string target in targetList)
				{
					mediaURL = this.GetOriginalImageUrl(target);
					uploadDate = this.GetUploadDateTime(content);
					fileName = MakeUniqueFileName(this._DownloadDIR
													+ @"\" + uploadDate
													+ "_" + this.SERVICE_NAME + "_"
													, target
													, target.Contains("mp4") ? MEDIATYPE.video : MEDIATYPE.image
													, files.Count
												).FullName;

					files.Add(new FileData(mediaURL, fileName, uploadDate));
				}

				return files;
			}
			catch (Exception ex)
			{
				Common.ShowExceptionMsgBox(ex);
				return null;
			}
		}

		public void StartDownload(string url)
		{
			try
			{
				List<FileData> files = PrefareDownload(url);

				// SetProgressbarMax
				_ControlerForm.SetProgressBarMaxValue(files.Count);

				// Start thread
				new Thread(() => Downloader.DownloadThread(files, this._ControlerForm)).Start();

			}
			catch (Exception ex)
			{
				Common.ShowExceptionMsgBox(ex);
			}
		}

		protected List<string> MakeUrlListFromContent(string WebContent)
		{
			try
			{
				if (String.IsNullOrEmpty(WebContent))
					return null;

				List<string> _filelist = GetUrlListFromContent(WebContent);

				if (_filelist.Count == 0)
					return null;
				else
					return _filelist;
			}
			catch (Exception ex)
			{
				Common.ShowExceptionMsgBox(ex);
				return null;
			}
		}

		public FileInfo MakeUniqueFileName(string path, string url, MEDIATYPE _MediaType, int currentCount)
		{
			string dir = Path.GetDirectoryName(path);
			string fileName = Path.GetFileNameWithoutExtension(path);
			string fileExt = _MediaType == MEDIATYPE.image ? ".jpg" : ".mp4";

			int existCount;
			for (existCount = 1; ; existCount++)
			{
				path = Path.Combine(dir, fileName + existCount.ToString("000") + fileExt);

				if (!File.Exists(path))
					break;
			}

			for (int i = existCount + currentCount; ; i++)
			{
				path = Path.Combine(dir, fileName + i.ToString("000") + fileExt);
				return new FileInfo(path);
			}
		}

		protected virtual string SetTargetUrl(string url)
		{
			return url;
		}

		protected virtual List<string> GetUrlListFromContent(string content)
		{
			throw new NotImplementedException();
		}

		protected virtual string GetOriginalImageUrl(string imgUrl)
		{
			return imgUrl;
		}

		protected virtual string GetOriginalVideoUrl(string videoUrl)
		{
			throw new NotImplementedException();
		}

		protected virtual string GetUploadDateTime(string content)
		{
			return DateTime.Now.ToString("yyyyMMdd");
		}
	}

	public class DownloadTwitter : ServiceDownload
	{
		public DownloadTwitter(IControlInterface MainFrm) : base(SERVICE.twitter, MainFrm) { }

		protected override List<string> GetUrlListFromContent(string content)
		{
			string _grepKeyword = @"og:image"" content="".*""\>";

			this.MEDIA_TYPE = MEDIATYPE.image; // Twitterの動画ダウンロードは今後追加するため
			MatchCollection tmp = Regex.Matches(content, _grepKeyword);
			List<string> tmpFileList = new List<string>();
			foreach (Match file in tmp)
			{
				tmpFileList.Add(Regex.Match(file.ToString(), @"https.*\.jpg").ToString());
			}

			return tmpFileList;
		}

		protected override string GetOriginalImageUrl(string imgUrl)
		{
			try
			{
				return imgUrl + ":orig";
			}
			catch (Exception ex)
			{
				Common.ShowExceptionMsgBox(ex);

				return string.Empty;
			}
		}

		protected override string GetUploadDateTime(string content)
		{
			string date_time_ms = Regex.Match(content, @"data-time-ms=""[0-9]+""").ToString();
			string value_of_date_time_ms = Regex.Match(date_time_ms, @"[0-9]+").ToString();
			long unixms = Convert.ToInt64(value_of_date_time_ms.ToString());
			DateTime uploadDT = Common.GetDateTimeFromUnixTime(unixms);
			return uploadDT.ToString("yyyyMMdd");
		}
	}

	public class DownloadInstagram : ServiceDownload
	{
		// https://www.instagram.com/p/BdcnRlSl4Yh
		public DownloadInstagram(IControlInterface MainFrm) : base(SERVICE.instagram, MainFrm) { }

		protected override List<string> GetUrlListFromContent(string content)
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

		protected override string SetTargetUrl(string url)
		{
			try
			{
				return Regex.Match(url, @"https\:\/\/www\.instagram\.com\/p\/[0-9a-zA-Z\-]+/?").ToString() + "?__a=1";
			}
			catch (Exception)
			{
				Common.ShowErrorMsgBox(url + "\r\n이 주소는 아직(?)지원하지 않습니다!");
				return null;
			}
		}

		protected override string GetUploadDateTime(string content)
		{
			List<string> tmpFileList = new List<string>();
			JObject jobj = JObject.Parse(content);
			string value_of_date_time_sec = jobj["graphql"]["shortcode_media"]["taken_at_timestamp"].ToString();
			int unixsec = Convert.ToInt32(value_of_date_time_sec.ToString()); // int32
			DateTime uploadDT = Common.GetDateTimeFromUnixTime(unixsec);
			return uploadDT.ToString("yyyyMMdd");
		}
	}
}
