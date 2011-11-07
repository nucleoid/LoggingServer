namespace LoggingServer.DbMigrations
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlNavigation = new System.Windows.Forms.Panel();
            this.btnFinish = new System.Windows.Forms.Button();
            this.pnlTopSplitter = new System.Windows.Forms.Panel();
            this.pnlBottomSplitter = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlNavigation.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbLogo
            // 
            this.pbLogo.Location = new System.Drawing.Point(12, 12);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(155, 50);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLogo.TabIndex = 0;
            this.pbLogo.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(407, 15);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Location = new System.Drawing.Point(0, 70);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(494, 253);
            this.pnlMain.TabIndex = 2;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.pbLogo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(494, 70);
            this.pnlHeader.TabIndex = 3;
            // 
            // pnlNavigation
            // 
            this.pnlNavigation.Controls.Add(this.btnFinish);
            this.pnlNavigation.Controls.Add(this.btnStart);
            this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlNavigation.Location = new System.Drawing.Point(0, 322);
            this.pnlNavigation.Name = "pnlNavigation";
            this.pnlNavigation.Size = new System.Drawing.Size(494, 50);
            this.pnlNavigation.TabIndex = 4;
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(407, 15);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 3;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Visible = false;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // pnlTopSplitter
            // 
            this.pnlTopSplitter.BackColor = System.Drawing.Color.Gray;
            this.pnlTopSplitter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopSplitter.Location = new System.Drawing.Point(0, 70);
            this.pnlTopSplitter.Name = "pnlTopSplitter";
            this.pnlTopSplitter.Size = new System.Drawing.Size(494, 1);
            this.pnlTopSplitter.TabIndex = 3;
            // 
            // pnlBottomSplitter
            // 
            this.pnlBottomSplitter.BackColor = System.Drawing.Color.Gray;
            this.pnlBottomSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomSplitter.Location = new System.Drawing.Point(0, 321);
            this.pnlBottomSplitter.Name = "pnlBottomSplitter";
            this.pnlBottomSplitter.Size = new System.Drawing.Size(494, 1);
            this.pnlBottomSplitter.TabIndex = 4;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 372);
            this.Controls.Add(this.pnlBottomSplitter);
            this.Controls.Add(this.pnlTopSplitter);
            this.Controls.Add(this.pnlNavigation);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 400);
            this.MinimumSize = new System.Drawing.Size(500, 28);
            this.Name = "Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Logging Server Database Update";
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlNavigation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlNavigation;
        private System.Windows.Forms.Panel pnlTopSplitter;
        private System.Windows.Forms.Panel pnlBottomSplitter;
        private System.Windows.Forms.Button btnFinish;
    }
}

