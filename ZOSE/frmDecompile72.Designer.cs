using System.Windows.Forms;
using System.Drawing;
using System;

namespace ZOSE
{
	partial class frmDecompile72
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.nID = new System.Windows.Forms.NumericUpDown();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nID)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(21, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "ID:";
			// 
			// nID
			// 
			this.nID.Hexadecimal = true;
			this.nID.Location = new System.Drawing.Point(39, 12);
			this.nID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nID.Name = "nID";
			this.nID.Size = new System.Drawing.Size(149, 20);
			this.nID.TabIndex = 1;
			this.nID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nID_KeyPress);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(93, 38);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(95, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Decompile";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(15, 38);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// frmDecompile72
			// 
			this.ClientSize = new System.Drawing.Size(200, 73);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.nID);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "frmDecompile72";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Decompile 72 Interaction";
			((System.ComponentModel.ISupportInitialize)(this.nID)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private Button button1;
		private Button button2;
		private Label label1;
		public NumericUpDown nID;

		#endregion
	}
}