using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WatchTool
{
	/// <summary>
	/// Common for Watchtool (and the others)
	/// </summary>
	public class Common
	{
		public static void ShowExceptionMsgBox(Exception ex)
		{
			MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
		}

		public static void ShowErrorMsgBox(string msg)
		{
			MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static DateTime GetDateTimeFromUnixTime(long unixTimeMilliSec)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddHours(9);
			return epoch.AddMilliseconds(unixTimeMilliSec);
		}

		public static DateTime GetDateTimeFromUnixTime(int unixTimeSec)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddHours(9);
			return epoch.AddSeconds(unixTimeSec);
		}
	}
}
