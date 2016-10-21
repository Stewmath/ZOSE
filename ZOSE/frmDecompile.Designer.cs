namespace ZOSE
{
	partial class frmDecompile
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
			this.nAddress = new System.Windows.Forms.NumericUpDown();
			this.button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nAddress)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Address:";
			// 
			// nAddress
			// 
			this.nAddress.Hexadecimal = true;
			this.nAddress.Location = new System.Drawing.Point(66, 12);
			this.nAddress.Maximum = new decimal(new int[] {
            2097151,
            0,
            0,
            0});
			this.nAddress.Name = "nAddress";
			this.nAddress.Size = new System.Drawing.Size(85, 20);
			this.nAddress.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(15, 38);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(136, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// frmDecompile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(163, 73);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.nAddress);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "frmDecompile";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Decompile";
			((System.ComponentModel.ISupportInitialize)(this.nAddress)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		public System.Windows.Forms.NumericUpDown nAddress;
	}
}