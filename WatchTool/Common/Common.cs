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
	}
}
