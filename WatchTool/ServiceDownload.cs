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
		//protected string _URL { get; set; }

		public ServiceDownload(SERVICE serviceName, IControlInterface controlerForm)
		{
			this.SERVICE_NAME = serviceName;
			this._ControlerForm = controlerForm;
		}

		public bool StartDownload(string url)
		{
			try
			{
				bool dl_result = false;

				if (string.IsNullOrEmpty(url))
					return false;
				else
					url = SetTargetUrl(url);

				string content = Downloader.GetContentsFromSrc(url);

				List<string> fileList = MakeFileList(content);

				if (fileList.Count >= 1)
				{
					dl_result = PrefareDownload(fileList);
				}

				return dl_result;
			}
			catch (Exception ex)
			{
				Common.ShowExceptionMsgBox(ex);
				return false;
			}
		}

		protected bool PrefareDownload(List<string> urlList)
		{
			// Download from web
			try
			{
				_ControlerForm.DoSetProgressBarMaxValue(urlList.Count); // SetProgressbarMax

				// Directory Initialize
				this._DownloadDIR = (string.IsNullOrEmpty(Properties.Settings.Default.DownloadDir)) ? Application.StartupPath + @"\Download" : Properties.Settings.Default.DownloadDir;
				DirectoryInfo di = new DirectoryInfo(_DownloadDIR);
				if (di.Exists == false)
				{
					di.Create();
				}

				List<FileData> files = new List<FileData>();
				string targetUrl = "";
				string fullName = "";
				foreach (string url in urlList)
				{
					targetUrl = this.GetOriginalImageUrl(url);
					fullName = MakeUniqueFileName(this._DownloadDIR
													+ @"\" + DateTime.Now.ToString("yyyyMMdd")
													+ "_" + this.SERVICE_NAME
													+ "_"
													, targetUrl
													, MEDIATYPE.image
													, files.Count
												).FullName;

					files.Add(new FileData(targetUrl, fullName));
				}

				Thread th = new Thread(() => Downloader.DownloadThread(files, this._ControlerForm));
				th.Start();

				return true;
			}
			catch (Exception ex)
			{
				Common.ShowExceptionMsgBox(ex);
				return false;
			}
		}

		protected List<string> MakeFileList(string WebContent)
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
			string fileExt = _MediaType == MEDIATYPE.image ? Path.GetExtension(url) : "mp4";

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
	}

	public class DownloadTwitter : ServiceDownload
	{
		string _grepKeyword = @"data-image-url=.*";

		public DownloadTwitter(IControlInterface MainFrm) : base(SERVICE.twitter, MainFrm) { }

		protected override List<string> GetUrlListFromContent(string content)
		{
			this.MEDIA_TYPE = MEDIATYPE.image; // Twitterの動画ダウンロードは今後追加するため
			MatchCollection tmp = System.Text.RegularExpressions.Regex.Matches(content, this._grepKeyword);
			List<string> tmpFileList = new List<string>();
			foreach (Match file in tmp)
			{
				tmpFileList.Add(file.ToString());
			}

			return tmpFileList;
		}

		protected override string GetOriginalImageUrl(string imgUrl)
		{
			// 末尾に :orig をつける
			// 引数 imgUrl の文字列の例 "data-image-url="https://pbs.twimg.com/media/DDp82xwUMAEDOpz.jpg""
			// やり方変えたい。。。
			try
			{
				string[] spltRst = imgUrl.Split('\"');
				if (spltRst.Length == 3)
					return spltRst[1] + ":orig";
			}
			catch (Exception ex)
			{
				Common.ShowExceptionMsgBox(ex);

				return string.Empty;
			}

			return string.Empty;
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
				return System.Text.RegularExpressions.Regex.Matches(url, @"https\:\/\/www\.instagram\.com\/p\/[0-9a-zA-Z\-]+/?")[0].ToString() + "?__a=1";
			}
			catch (Exception)
			{
				Common.ShowErrorMsgBox(url + "\r\n이 주소는 아직(?)지원하지 않습니다!");
				return null;
			}
		}
	}
}
