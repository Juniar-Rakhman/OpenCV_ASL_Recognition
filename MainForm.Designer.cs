namespace OpenCVLib2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerFPS = new System.Windows.Forms.Timer(this.components);
            this.textBox = new System.Windows.Forms.TextBox();
            this.picBoxMain = new System.Windows.Forms.PictureBox();
            this.timerGrab = new System.Windows.Forms.Timer(this.components);
            this.txtK = new System.Windows.Forms.TextBox();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openVideoFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.trainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.skinDetectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rGBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.featureExtractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thresholdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edgeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cropWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.motionWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noiseRemovalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.additionalInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("statusStrip.BackgroundImage")));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 583);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(645, 22);
            this.statusStrip.Stretch = false;
            this.statusStrip.TabIndex = 18;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // timerFPS
            // 
            this.timerFPS.Tick += new System.EventHandler(this.timerFPS_Tick);
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.ForeColor = System.Drawing.Color.Green;
            this.textBox.Location = new System.Drawing.Point(0, 481);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(645, 102);
            this.textBox.TabIndex = 21;
            this.textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            // 
            // picBoxMain
            // 
            this.picBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxMain.Location = new System.Drawing.Point(0, 35);
            this.picBoxMain.Name = "picBoxMain";
            this.picBoxMain.Size = new System.Drawing.Size(645, 446);
            this.picBoxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBoxMain.TabIndex = 22;
            this.picBoxMain.TabStop = false;
            // 
            // timerGrab
            // 
            this.timerGrab.Tick += new System.EventHandler(this.timerGrab_Tick);
            // 
            // txtK
            // 
            this.txtK.Location = new System.Drawing.Point(599, 9);
            this.txtK.Name = "txtK";
            this.txtK.Size = new System.Drawing.Size(23, 20);
            this.txtK.TabIndex = 23;
            this.txtK.Text = "18";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openVideoFileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 31);
            this.fileToolStripMenuItem.Text = "File...";
            // 
            // openVideoFileToolStripMenuItem
            // 
            this.openVideoFileToolStripMenuItem.Name = "openVideoFileToolStripMenuItem";
            this.openVideoFileToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.openVideoFileToolStripMenuItem.Text = "Open Video File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // btnVideo
            // 
            this.btnVideo.BackColor = System.Drawing.Color.Transparent;
            this.btnVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVideo.Image = ((System.Drawing.Image)(resources.GetObject("btnVideo.Image")));
            this.btnVideo.ImageTransparentColor = System.Drawing.Color.White;
            this.btnVideo.Name = "btnVideo";
            this.btnVideo.Size = new System.Drawing.Size(87, 31);
            this.btnVideo.Text = "Start Video";
            this.btnVideo.Click += new System.EventHandler(this.btnVideo_Click);
            // 
            // trainToolStripMenuItem
            // 
            this.trainToolStripMenuItem.Name = "trainToolStripMenuItem";
            this.trainToolStripMenuItem.Size = new System.Drawing.Size(46, 31);
            this.trainToolStripMenuItem.Text = "Train";
            this.trainToolStripMenuItem.Click += new System.EventHandler(this.trainToolStripMenuItem_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.AllowMerge = false;
            this.menuStrip.AutoSize = false;
            this.menuStrip.BackColor = System.Drawing.Color.LightGray;
            this.menuStrip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("menuStrip.BackgroundImage")));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.skinDetectToolStripMenuItem,
            this.featureExtractToolStripMenuItem,
            this.displayToolStripMenuItem,
            this.trainToolStripMenuItem,
            this.btnVideo});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(645, 35);
            this.menuStrip.TabIndex = 19;
            this.menuStrip.Text = "menuStrip1";
            // 
            // skinDetectToolStripMenuItem
            // 
            this.skinDetectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hSVToolStripMenuItem,
            this.rGBToolStripMenuItem});
            this.skinDetectToolStripMenuItem.Name = "skinDetectToolStripMenuItem";
            this.skinDetectToolStripMenuItem.Size = new System.Drawing.Size(87, 31);
            this.skinDetectToolStripMenuItem.Text = "Skin Detect...";
            // 
            // hSVToolStripMenuItem
            // 
            this.hSVToolStripMenuItem.Checked = true;
            this.hSVToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hSVToolStripMenuItem.Name = "hSVToolStripMenuItem";
            this.hSVToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.hSVToolStripMenuItem.Text = "HSV";
            this.hSVToolStripMenuItem.Click += new System.EventHandler(this.hSVToolStripMenuItem_Click);
            // 
            // rGBToolStripMenuItem
            // 
            this.rGBToolStripMenuItem.Name = "rGBToolStripMenuItem";
            this.rGBToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.rGBToolStripMenuItem.Text = "RGB";
            this.rGBToolStripMenuItem.Click += new System.EventHandler(this.rGBToolStripMenuItem_Click);
            // 
            // featureExtractToolStripMenuItem
            // 
            this.featureExtractToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thresholdToolStripMenuItem,
            this.edgeToolStripMenuItem});
            this.featureExtractToolStripMenuItem.Name = "featureExtractToolStripMenuItem";
            this.featureExtractToolStripMenuItem.Size = new System.Drawing.Size(67, 31);
            this.featureExtractToolStripMenuItem.Text = "Feature...";
            // 
            // thresholdToolStripMenuItem
            // 
            this.thresholdToolStripMenuItem.Checked = true;
            this.thresholdToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.thresholdToolStripMenuItem.Name = "thresholdToolStripMenuItem";
            this.thresholdToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.thresholdToolStripMenuItem.Text = "Threshold";
            // 
            // edgeToolStripMenuItem
            // 
            this.edgeToolStripMenuItem.Name = "edgeToolStripMenuItem";
            this.edgeToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.edgeToolStripMenuItem.Text = "Edge";
            this.edgeToolStripMenuItem.Click += new System.EventHandler(this.edgeToolStripMenuItem_Click);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cropWindowToolStripMenuItem,
            this.motionWindowToolStripMenuItem,
            this.noiseRemovalToolStripMenuItem,
            this.additionalInfoToolStripMenuItem});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(66, 31);
            this.displayToolStripMenuItem.Text = "Display...";
            // 
            // cropWindowToolStripMenuItem
            // 
            this.cropWindowToolStripMenuItem.Checked = true;
            this.cropWindowToolStripMenuItem.CheckOnClick = true;
            this.cropWindowToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cropWindowToolStripMenuItem.Name = "cropWindowToolStripMenuItem";
            this.cropWindowToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.cropWindowToolStripMenuItem.Text = "Crop Window";
            // 
            // motionWindowToolStripMenuItem
            // 
            this.motionWindowToolStripMenuItem.Checked = true;
            this.motionWindowToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.motionWindowToolStripMenuItem.Name = "motionWindowToolStripMenuItem";
            this.motionWindowToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.motionWindowToolStripMenuItem.Text = "Motion Window";
            // 
            // noiseRemovalToolStripMenuItem
            // 
            this.noiseRemovalToolStripMenuItem.Checked = true;
            this.noiseRemovalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noiseRemovalToolStripMenuItem.Name = "noiseRemovalToolStripMenuItem";
            this.noiseRemovalToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.noiseRemovalToolStripMenuItem.Text = "Noise Removal";
            // 
            // additionalInfoToolStripMenuItem
            // 
            this.additionalInfoToolStripMenuItem.Name = "additionalInfoToolStripMenuItem";
            this.additionalInfoToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.additionalInfoToolStripMenuItem.Text = "Additional Info";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(579, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "K";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(645, 605);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtK);
            this.Controls.Add(this.picBoxMain);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Location = new System.Drawing.Point(10, 10);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Vision C#";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerFPS;
        public System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.PictureBox picBoxMain;
        private System.Windows.Forms.Timer timerGrab;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openVideoFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnVideo;
        private System.Windows.Forms.ToolStripMenuItem trainToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        public System.Windows.Forms.TextBox txtK;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.StatusStrip statusStrip;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem skinDetectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rGBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem featureExtractToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thresholdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem edgeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cropWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem motionWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noiseRemovalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem additionalInfoToolStripMenuItem;
    }
}

