namespace ZOSE
{
	partial class frmFreeSpace
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
			this.rbBank = new System.Windows.Forms.RadioButton();
			this.nBank = new System.Windows.Forms.NumericUpDown();
			this.rbAnywhere = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.nSpace = new System.Windows.Forms.NumericUpDown();
			this.button1 = new System.Windows.Forms.Button();
			this.lstSpace = new System.Windows.Forms.ListBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.nResults = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nBank)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nSpace)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nResults)).BeginInit();
			this.SuspendLayout();
			// 
			// rbBank
			// 
			this.rbBank.AutoSize = true;
			this.rbBank.Checked = true;
			this.rbBank.Location = new System.Drawing.Point(12, 12);
			this.rbBank.Name = "rbBank";
			this.rbBank.Size = new System.Drawing.Size(53, 17);
			this.rbBank.TabIndex = 0;
			this.rbBank.TabStop = true;
			this.rbBank.Text = "Bank:";
			this.rbBank.UseVisualStyleBackColor = true;
			this.rbBank.CheckedChanged += new System.EventHandler(this.rbBank_CheckedChanged);
			// 
			// nBank
			// 
			this.nBank.Hexadecimal = true;
			this.nBank.Location = new System.Drawing.Point(71, 12);
			this.nBank.Name = "nBank";
			this.nBank.Size = new System.Drawing.Size(98, 20);
			this.nBank.TabIndex = 1;
			this.nBank.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
			// 
			// rbAnywhere
			// 
			this.rbAnywhere.AutoSize = true;
			this.rbAnywhere.Location = new System.Drawing.Point(12, 38);
			this.rbAnywhere.Name = "rbAnywhere";
			this.rbAnywhere.Size = new System.Drawing.Size(72, 17);
			this.rbAnywhere.TabIndex = 2;
			this.rbAnywhere.Text = "Anywhere";
			this.rbAnywhere.UseVisualStyleBackColor = true;
			this.rbAnywhere.CheckedChanged += new System.EventHandler(this.rbAnywhere_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 63);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Space:";
			// 
			// nSpace
			// 
			this.nSpace.Hexadecimal = true;
			this.nSpace.Location = new System.Drawing.Point(60, 61);
			this.nSpace.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.nSpace.Name = "nSpace";
			this.nSpace.Size = new System.Drawing.Size(109, 20);
			this.nSpace.TabIndex = 4;
			this.nSpace.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 118);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(157, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "Search";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// lstSpace
			// 
			this.lstSpace.FormattingEnabled = true;
			this.lstSpace.Location = new System.Drawing.Point(12, 147);
			this.lstSpace.Name = "lstSpace";
			this.lstSpace.ScrollAlwaysVisible = true;
			this.lstSpace.Size = new System.Drawing.Size(157, 95);
			this.lstSpace.TabIndex = 6;
			this.lstSpace.SelectedIndexChanged += new System.EventHandler(this.lstSpace_SelectedIndexChanged);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(87, 248);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(82, 23);
			this.button2.TabIndex = 7;
			this.button2.Text = "OK";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(12, 248);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(69, 23);
			this.button3.TabIndex = 8;
			this.button3.Text = "Cancel";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// nResults
			// 
			this.nResults.Location = new System.Drawing.Point(60, 92);
			this.nResults.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nResults.Name = "nResults";
			this.nResults.Size = new System.Drawing.Size(109, 20);
			this.nResults.TabIndex = 10;
			this.nResults.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 94);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Results:";
			// 
			// frmFreeSpace
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(181, 283);
			this.Controls.Add(this.nResults);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.lstSpace);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.nSpace);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.rbAnywhere);
			this.Controls.Add(this.nBank);
			this.Controls.Add(this.rbBank);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "frmFreeSpace";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Find Free Space";
			((System.ComponentModel.ISupportInitialize)(this.nBank)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nSpace)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nResults)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton rbBank;
		private System.Windows.Forms.NumericUpDown nBank;
		private System.Windows.Forms.RadioButton rbAnywhere;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nSpace;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		public System.Windows.Forms.ListBox lstSpace;
		private System.Windows.Forms.NumericUpDown nResults;
		private System.Windows.Forms.Label label2;
	}
}