namespace ZOSE
{
    partial class frmDecompileInteraction
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
            this.nDungeon = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.nID = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nX = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nY = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nDungeon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nY)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dungeon:";
            // 
            // nDungeon
            // 
            this.nDungeon.Hexadecimal = true;
            this.nDungeon.Location = new System.Drawing.Point(72, 12);
            this.nDungeon.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nDungeon.Name = "nDungeon";
            this.nDungeon.Size = new System.Drawing.Size(79, 20);
            this.nDungeon.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // nID
            // 
            this.nID.Hexadecimal = true;
            this.nID.Location = new System.Drawing.Point(72, 38);
            this.nID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nID.Name = "nID";
            this.nID.Size = new System.Drawing.Size(79, 20);
            this.nID.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "ID:";
            // 
            // nX
            // 
            this.nX.Hexadecimal = true;
            this.nX.Location = new System.Drawing.Point(72, 64);
            this.nX.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nX.Name = "nX";
            this.nX.Size = new System.Drawing.Size(79, 20);
            this.nX.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "X:";
            // 
            // nY
            // 
            this.nY.Hexadecimal = true;
            this.nY.Location = new System.Drawing.Point(72, 90);
            this.nY.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nY.Name = "nY";
            this.nY.Size = new System.Drawing.Size(79, 20);
            this.nY.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Y:";
            // 
            // frmDecompileInteraction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(163, 149);
            this.Controls.Add(this.nY);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.nDungeon);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmDecompileInteraction";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Decompile";
            this.Load += new System.EventHandler(this.frmDecompile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nDungeon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.NumericUpDown nDungeon;
        public System.Windows.Forms.NumericUpDown nID;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown nX;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.NumericUpDown nY;
        private System.Windows.Forms.Label label4;
    }
}