using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace ZOSE
{
	public partial class frmFreeSpace : Form
	{
		GBHL.GBFile gb;
		private int spaceLocation = -1;
		public int SpaceLocation
		{
			get { return spaceLocation; }
			set { spaceLocation = value; }
		}
		public frmFreeSpace(GBHL.GBFile g)
		{
			InitializeComponent();
			gb = g;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			lstSpace.Items.Clear();
			spaceLocation = -1;
			int last = -1;
			for (int i = 0; i < (int)nResults.Value; i++)
			{
				int space = FindFreeSpace((i == 0 ? -1 : last + (int)nSpace.Value), (int)nSpace.Value, (int)(rbBank.Checked ? (int)nBank.Value : -1));
				if (space != -1)
					lstSpace.Items.Add(space.ToString("X"));
				else
					break;
				last = space + 1;
			}
		}

		public int FindFreeSpace(int start, int amount, int bank)
		{
			int found = 0;
			if (start != -1)
			{
				gb.BufferLocation = start;
				if (gb.BufferLocation < bank * 0x4000)
					gb.BufferLocation += bank * 0x4000;
			}
			else
			{
				if (bank != -1)
				{
					gb.BufferLocation = bank * 0x4000;
					if (bank == 0)
						gb.BufferLocation += 0x4000;
				}
				else
					gb.BufferLocation = 0x4000;
			}
			FindSpace:
			while (found < amount)
			{
				if (gb.BufferLocation == gb.Buffer.Length - 1 || (bank != -1 && gb.BufferLocation > bank * 0x4000 + 0x3FFF))
				{
					return -1;
				}
				if (gb.BufferLocation == 0x33FA0 || gb.ReadByte() != 0)
				{
					if (gb.BufferLocation == 0x33FA0)
						gb.BufferLocation++;
					found = 0;
					continue;
				}
				if (gb.ReadByte() == 0)
				{
					if (found > 0)
						gb.BufferLocation--;
					else if(bank == -1)
					{
						if (gb.ReadByte() != 0)
							continue;
					}
					found++;
					if (gb.BufferLocation % 0x4000 == 0)
						found = 0;
				}
			}
			//Double check...
			int end = gb.BufferLocation - found - 1;
			gb.BufferLocation = end;
			for (int i = 0; i < amount; i++)
			{
				if (gb.ReadByte() != 0)
				{
					gb.BufferLocation = end + amount;
					found = 0;
					goto FindSpace;
				}
			}

			return end;
		}

		private void lstSpace_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstSpace.SelectedIndex == -1)
				spaceLocation = -1;
			spaceLocation = int.Parse((string)lstSpace.SelectedItem, System.Globalization.NumberStyles.HexNumber);
		}

		private void rbAnywhere_CheckedChanged(object sender, EventArgs e)
		{
			
		}

		private void rbBank_CheckedChanged(object sender, EventArgs e)
		{
			if (rbBank.Checked)
			{
				nSpace.Value = 5;
			}
			else
			{
				nSpace.Value = 256;
			}
		}
	}
}
