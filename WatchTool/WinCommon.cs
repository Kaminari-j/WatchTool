using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WatchTool
{
	class WinCommon
	{
		public static void sortListBoxItems(ref ListBox lb, bool descending)
		{
			List<object> items;
			items = lb.Items.OfType<object>().ToList();
			lb.Items.Clear();
			if (descending)
			{ lb.Items.AddRange(items.OrderByDescending(i => i).ToArray()); }
			else
			{ lb.Items.AddRange(items.OrderBy(i => i).ToArray()); }
		}
	}
}