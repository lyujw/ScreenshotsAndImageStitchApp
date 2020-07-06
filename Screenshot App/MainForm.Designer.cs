namespace Screenshot_App {
    partial class MainForm {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.GetPicButton1 = new System.Windows.Forms.Button();
            this.GetPicButton2 = new System.Windows.Forms.Button();
            this.GetPicButton3 = new System.Windows.Forms.Button();
            this.GetPicButton4 = new System.Windows.Forms.Button();
            this.SplicingButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GetPicButton1
            // 
            this.GetPicButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("GetPicButton1.BackgroundImage")));
            this.GetPicButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GetPicButton1.Location = new System.Drawing.Point(72, 41);
            this.GetPicButton1.Name = "GetPicButton1";
            this.GetPicButton1.Size = new System.Drawing.Size(162, 135);
            this.GetPicButton1.TabIndex = 0;
            this.GetPicButton1.UseVisualStyleBackColor = true;
            this.GetPicButton1.Click += new System.EventHandler(this.GetPicButton1_Click);
            // 
            // GetPicButton2
            // 
            this.GetPicButton2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("GetPicButton2.BackgroundImage")));
            this.GetPicButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GetPicButton2.Location = new System.Drawing.Point(362, 41);
            this.GetPicButton2.Name = "GetPicButton2";
            this.GetPicButton2.Size = new System.Drawing.Size(162, 135);
            this.GetPicButton2.TabIndex = 1;
            this.GetPicButton2.UseVisualStyleBackColor = true;
            this.GetPicButton2.Click += new System.EventHandler(this.GetPicButton2_Click);
            // 
            // GetPicButton3
            // 
            this.GetPicButton3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("GetPicButton3.BackgroundImage")));
            this.GetPicButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GetPicButton3.Location = new System.Drawing.Point(72, 223);
            this.GetPicButton3.Name = "GetPicButton3";
            this.GetPicButton3.Size = new System.Drawing.Size(162, 135);
            this.GetPicButton3.TabIndex = 2;
            this.GetPicButton3.UseVisualStyleBackColor = true;
            this.GetPicButton3.Click += new System.EventHandler(this.GetPicButton3_Click);
            // 
            // GetPicButton4
            // 
            this.GetPicButton4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("GetPicButton4.BackgroundImage")));
            this.GetPicButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GetPicButton4.Location = new System.Drawing.Point(362, 223);
            this.GetPicButton4.Name = "GetPicButton4";
            this.GetPicButton4.Size = new System.Drawing.Size(162, 135);
            this.GetPicButton4.TabIndex = 3;
            this.GetPicButton4.UseVisualStyleBackColor = true;
            this.GetPicButton4.Click += new System.EventHandler(this.GetPicButton4_Click);
            // 
            // SplicingButton
            // 
            this.SplicingButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.SplicingButton.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SplicingButton.Location = new System.Drawing.Point(209, 410);
            this.SplicingButton.Name = "SplicingButton";
            this.SplicingButton.Size = new System.Drawing.Size(178, 64);
            this.SplicingButton.TabIndex = 5;
            this.SplicingButton.Text = "Splicing";
            this.SplicingButton.UseVisualStyleBackColor = false;
            this.SplicingButton.Click += new System.EventHandler(this.SplicingButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(602, 30);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 26);
            this.helpToolStripMenuItem.Text = "help";
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.manualToolStripMenuItem.Text = "manual";
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.manualToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(602, 524);
            this.Controls.Add(this.SplicingButton);
            this.Controls.Add(this.GetPicButton4);
            this.Controls.Add(this.GetPicButton3);
            this.Controls.Add(this.GetPicButton2);
            this.Controls.Add(this.GetPicButton1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Screenshot App V1.0";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetPicButton1;
        private System.Windows.Forms.Button GetPicButton2;
        private System.Windows.Forms.Button GetPicButton3;
        private System.Windows.Forms.Button GetPicButton4;
        private System.Windows.Forms.Button SplicingButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
    }
}

