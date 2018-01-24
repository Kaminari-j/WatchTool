using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchTool
{
	public class FileData
	{
		public string URL { get; set; }
		public string FileName { get; set; }
		public string UploadDate { get; set; }

		public FileData(string URL, string FileName)
		{
			this.URL = URL;
			this.FileName = FileName;
		}

		public FileData(string URL, string FileName, string UploadDate) : this(URL, FileName)
		{
			this.UploadDate = UploadDate;
		}
	}

	//public class FileDataList : List<FileData>, IEnumerable<FileData>
	//{
	//	List<FileData> list = new List<FileData>();

	//	//public void Add(string URL, string FileName)
	//	//{
	//	//	list.Add(new FileData(URL, FileName));
	//	//}

	//	//public void Add(FileData FileData)
	//	//{
	//	//	list.Add(FileData);
	//	//}

	//	//public FileData this[int index]
	//	//{
	//	//	get { return list[index]; }
	//	//	set { list.Insert(index, value); }
	//	//}

	//	//public IEnumerator<FileData> GetEnumerator()
	//	//{
	//	//	return list.GetEnumerator();
	//	//}

	//	IEnumerator IEnumerable.GetEnumerator()
	//	{
	//		return this.GetEnumerator();
	//	}
	//}
}
