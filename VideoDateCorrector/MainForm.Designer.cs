namespace VideoDateCorrector
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMOVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMOVDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoDisplayTB = new System.Windows.Forms.RichTextBox();
            this.phoneNameDisplayTB = new System.Windows.Forms.RichTextBox();
            this.updateFileBtn = new System.Windows.Forms.Button();
            this.updateDirectoryBtn = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(551, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMOVToolStripMenuItem,
            this.openMOVDirectoryToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openMOVToolStripMenuItem
            // 
            this.openMOVToolStripMenuItem.Name = "openMOVToolStripMenuItem";
            this.openMOVToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMOVToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openMOVToolStripMenuItem.Text = "Open MOV";
            this.openMOVToolStripMenuItem.Click += new System.EventHandler(this.openMOVToolStripMenuItem_Click);
            // 
            // openMOVDirectoryToolStripMenuItem
            // 
            this.openMOVDirectoryToolStripMenuItem.Name = "openMOVDirectoryToolStripMenuItem";
            this.openMOVDirectoryToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openMOVDirectoryToolStripMenuItem.Text = "Open MOV Directory";
            this.openMOVDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openMOVDirectoryToolStripMenuItem_Click);
            // 
            // infoDisplayTB
            // 
            this.infoDisplayTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoDisplayTB.Location = new System.Drawing.Point(13, 28);
            this.infoDisplayTB.Name = "infoDisplayTB";
            this.infoDisplayTB.ReadOnly = true;
            this.infoDisplayTB.Size = new System.Drawing.Size(526, 166);
            this.infoDisplayTB.TabIndex = 1;
            this.infoDisplayTB.Text = "";
            // 
            // phoneNameDisplayTB
            // 
            this.phoneNameDisplayTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phoneNameDisplayTB.Location = new System.Drawing.Point(13, 219);
            this.phoneNameDisplayTB.Name = "phoneNameDisplayTB";
            this.phoneNameDisplayTB.ReadOnly = true;
            this.phoneNameDisplayTB.Size = new System.Drawing.Size(526, 46);
            this.phoneNameDisplayTB.TabIndex = 2;
            this.phoneNameDisplayTB.Text = "";
            // 
            // updateFileBtn
            // 
            this.updateFileBtn.Location = new System.Drawing.Point(89, 332);
            this.updateFileBtn.Name = "updateFileBtn";
            this.updateFileBtn.Size = new System.Drawing.Size(117, 37);
            this.updateFileBtn.TabIndex = 3;
            this.updateFileBtn.Text = "Update File Date";
            this.updateFileBtn.UseVisualStyleBackColor = true;
            // 
            // updateDirectoryBtn
            // 
            this.updateDirectoryBtn.Location = new System.Drawing.Point(331, 332);
            this.updateDirectoryBtn.Name = "updateDirectoryBtn";
            this.updateDirectoryBtn.Size = new System.Drawing.Size(117, 37);
            this.updateDirectoryBtn.TabIndex = 4;
            this.updateDirectoryBtn.Text = "Update All In Current Directory";
            this.updateDirectoryBtn.UseVisualStyleBackColor = true;
            this.updateDirectoryBtn.Click += new System.EventHandler(this.updateDirectoryBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 407);
            this.Controls.Add(this.updateDirectoryBtn);
            this.Controls.Add(this.updateFileBtn);
            this.Controls.Add(this.phoneNameDisplayTB);
            this.Controls.Add(this.infoDisplayTB);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "VideoDateCorrector";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMOVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMOVDirectoryToolStripMenuItem;
        private System.Windows.Forms.RichTextBox infoDisplayTB;
        private System.Windows.Forms.RichTextBox phoneNameDisplayTB;
        private System.Windows.Forms.Button updateFileBtn;
        private System.Windows.Forms.Button updateDirectoryBtn;
    }
}

