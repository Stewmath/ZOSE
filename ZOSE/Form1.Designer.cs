namespace ZOSE
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.showIntellisenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reApplyASMPatchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.applyNewOpcodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.builToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.decompileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.decompileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.decompileInteractionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lstIntellisense = new System.Windows.Forms.ListBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.decompile72InteractionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolStripMenuItem2,
            this.builToolStripMenuItem,
            this.decompileToolStripMenuItem,
            this.editToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.menuStrip1.Size = new System.Drawing.Size(592, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openROMToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem7,
            this.toolStripMenuItem5,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripSeparator1,
            this.toolStripMenuItem6});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openROMToolStripMenuItem
			// 
			this.openROMToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openROMToolStripMenuItem.Image")));
			this.openROMToolStripMenuItem.Name = "openROMToolStripMenuItem";
			this.openROMToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openROMToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
			this.openROMToolStripMenuItem.Text = "Open ROM...";
			this.openROMToolStripMenuItem.Click += new System.EventHandler(this.openROMToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(220, 6);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem7.Image")));
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.toolStripMenuItem7.Size = new System.Drawing.Size(223, 22);
			this.toolStripMenuItem7.Text = "New Script";
			this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.O)));
			this.toolStripMenuItem5.Size = new System.Drawing.Size(223, 22);
			this.toolStripMenuItem5.Text = "Open Script...";
			this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem3.Image")));
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.toolStripMenuItem3.Size = new System.Drawing.Size(223, 22);
			this.toolStripMenuItem3.Text = "Save Script";
			this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.S)));
			this.toolStripMenuItem4.Size = new System.Drawing.Size(223, 22);
			this.toolStripMenuItem4.Text = "Save Script As...";
			this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(220, 6);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem6.Image")));
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.toolStripMenuItem6.Size = new System.Drawing.Size(223, 22);
			this.toolStripMenuItem6.Text = "Find Free Space...";
			this.toolStripMenuItem6.Click += new System.EventHandler(this.findFreeSpaceToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showIntellisenseToolStripMenuItem,
            this.reApplyASMPatchesToolStripMenuItem,
            this.applyNewOpcodesToolStripMenuItem});
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(44, 20);
			this.toolStripMenuItem2.Text = "&Tools";
			// 
			// showIntellisenseToolStripMenuItem
			// 
			this.showIntellisenseToolStripMenuItem.Name = "showIntellisenseToolStripMenuItem";
			this.showIntellisenseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
			this.showIntellisenseToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
			this.showIntellisenseToolStripMenuItem.Text = "Show/Hide Intellisense";
			this.showIntellisenseToolStripMenuItem.Click += new System.EventHandler(this.showIntellisenseToolStripMenuItem_Click);
			// 
			// reApplyASMPatchesToolStripMenuItem
			// 
			this.reApplyASMPatchesToolStripMenuItem.Name = "reApplyASMPatchesToolStripMenuItem";
			this.reApplyASMPatchesToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
			this.reApplyASMPatchesToolStripMenuItem.Text = "Re-Apply ASM Patches";
			this.reApplyASMPatchesToolStripMenuItem.Click += new System.EventHandler(this.reApplyASMPatchesToolStripMenuItem_Click);
			// 
			// applyNewOpcodesToolStripMenuItem
			// 
			this.applyNewOpcodesToolStripMenuItem.Name = "applyNewOpcodesToolStripMenuItem";
			this.applyNewOpcodesToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
			this.applyNewOpcodesToolStripMenuItem.Text = "Apply New Opcodes";
			this.applyNewOpcodesToolStripMenuItem.Click += new System.EventHandler(this.applyNewOpcodesToolStripMenuItem_Click);
			// 
			// builToolStripMenuItem
			// 
			this.builToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileToolStripMenuItem});
			this.builToolStripMenuItem.Name = "builToolStripMenuItem";
			this.builToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.builToolStripMenuItem.Text = "&Build";
			// 
			// compileToolStripMenuItem
			// 
			this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
			this.compileToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.compileToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.compileToolStripMenuItem.Text = "Compile";
			this.compileToolStripMenuItem.Click += new System.EventHandler(this.compileToolStripMenuItem_Click);
			// 
			// decompileToolStripMenuItem
			// 
			this.decompileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decompileToolStripMenuItem1,
            this.decompileInteractionToolStripMenuItem,
            this.decompile72InteractionToolStripMenuItem});
			this.decompileToolStripMenuItem.Name = "decompileToolStripMenuItem";
			this.decompileToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
			this.decompileToolStripMenuItem.Text = "&Decompile";
			// 
			// decompileToolStripMenuItem1
			// 
			this.decompileToolStripMenuItem1.Name = "decompileToolStripMenuItem1";
			this.decompileToolStripMenuItem1.Size = new System.Drawing.Size(205, 22);
			this.decompileToolStripMenuItem1.Text = "Decompile Address...";
			this.decompileToolStripMenuItem1.Click += new System.EventHandler(this.decompileToolStripMenuItem1_Click);
			// 
			// decompileInteractionToolStripMenuItem
			// 
			this.decompileInteractionToolStripMenuItem.Name = "decompileInteractionToolStripMenuItem";
			this.decompileInteractionToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.decompileInteractionToolStripMenuItem.Text = "Decompile Interaction...";
			this.decompileInteractionToolStripMenuItem.Click += new System.EventHandler(this.decompileInteractionToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			this.editToolStripMenuItem.Visible = false;
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.selectAllToolStripMenuItem.Text = "Select All";
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lstIntellisense);
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(592, 429);
			this.panel1.TabIndex = 1;
			// 
			// lstIntellisense
			// 
			this.lstIntellisense.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstIntellisense.FormattingEnabled = true;
			this.lstIntellisense.ItemHeight = 16;
			this.lstIntellisense.Location = new System.Drawing.Point(8, 8);
			this.lstIntellisense.Name = "lstIntellisense";
			this.lstIntellisense.Size = new System.Drawing.Size(201, 132);
			this.lstIntellisense.Sorted = true;
			this.lstIntellisense.TabIndex = 1;
			this.lstIntellisense.Visible = false;
			this.lstIntellisense.DoubleClick += new System.EventHandler(this.lstIntellisense_DoubleClick);
			this.lstIntellisense.Click += new System.EventHandler(this.lstIntellisense_Click);
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.MaxLength = 65535;
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(592, 429);
			this.textBox1.TabIndex = 0;
			this.textBox1.WordWrap = false;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
			this.textBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
			this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
			// 
			// decompile72InteractionToolStripMenuItem
			// 
			this.decompile72InteractionToolStripMenuItem.Name = "decompile72InteractionToolStripMenuItem";
			this.decompile72InteractionToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.decompile72InteractionToolStripMenuItem.Text = "Decompile 72 Interaction...";
			this.decompile72InteractionToolStripMenuItem.Click += new System.EventHandler(this.decompile72InteractionToolStripMenuItem_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(592, 453);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ZOSE - Zelda Oracles Script Editor";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openROMToolStripMenuItem;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStripMenuItem builToolStripMenuItem;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem decompileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem decompileToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem decompileInteractionToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ListBox lstIntellisense;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem showIntellisenseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reApplyASMPatchesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem applyNewOpcodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem decompile72InteractionToolStripMenuItem;
	}
}

