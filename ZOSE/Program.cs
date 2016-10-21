using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace ZOSE
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			string filename = "";
			if (args != null)
			{
				foreach(string s in args)
					filename += s + " ";
			}
			if (filename != "")
				filename = filename.Substring(0, filename.Length - 1);
			Application.Run(new Form1(filename));
		}
	}
}
