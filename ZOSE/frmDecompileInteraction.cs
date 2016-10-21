using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace ZOSE
{
	public partial class frmDecompileInteraction : Form
	{
		public frmDecompileInteraction()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void frmDecompile_Load(object sender, EventArgs e)
		{

		}
	}
}
