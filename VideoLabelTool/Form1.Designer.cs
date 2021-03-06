
using System.Windows.Forms;

namespace VideoLabelTool
{
    partial class FormFrameCapture
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
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bntLoadLabels = new System.Windows.Forms.Button();
            this.bntPrevFrame = new System.Windows.Forms.Button();
            this.bntNextFrame = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bntPause = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.bntWalking = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bntDrinking = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(944, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.bntDrinking);
            this.splitContainer1.Panel1.Controls.Add(this.bntWalking);
            this.splitContainer1.Panel1.Controls.Add(this.bntLoadLabels);
            this.splitContainer1.Panel1.Controls.Add(this.bntPrevFrame);
            this.splitContainer1.Panel1.Controls.Add(this.bntNextFrame);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.bntPause);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Size = new System.Drawing.Size(944, 493);
            this.splitContainer1.SplitterDistance = 100;
            this.splitContainer1.TabIndex = 1;
            // 
            // bntLoadLabels
            // 
            this.bntLoadLabels.Location = new System.Drawing.Point(462, 29);
            this.bntLoadLabels.Name = "bntLoadLabels";
            this.bntLoadLabels.Size = new System.Drawing.Size(75, 23);
            this.bntLoadLabels.TabIndex = 2;
            this.bntLoadLabels.Text = "Load";
            this.bntLoadLabels.UseVisualStyleBackColor = true;
            this.bntLoadLabels.Click += new System.EventHandler(this.bntLoadLabels_Click);
            // 
            // bntPrevFrame
            // 
            this.bntPrevFrame.Location = new System.Drawing.Point(241, 53);
            this.bntPrevFrame.Name = "bntPrevFrame";
            this.bntPrevFrame.Size = new System.Drawing.Size(84, 23);
            this.bntPrevFrame.TabIndex = 5;
            this.bntPrevFrame.Text = "Previous F.";
            this.bntPrevFrame.UseVisualStyleBackColor = true;
            this.bntPrevFrame.Click += new System.EventHandler(this.bntPrevFrame_Click);
            // 
            // bntNextFrame
            // 
            this.bntNextFrame.Location = new System.Drawing.Point(355, 53);
            this.bntNextFrame.Name = "bntNextFrame";
            this.bntNextFrame.Size = new System.Drawing.Size(75, 23);
            this.bntNextFrame.TabIndex = 4;
            this.bntNextFrame.Text = "Next F.";
            this.bntNextFrame.UseVisualStyleBackColor = true;
            this.bntNextFrame.Click += new System.EventHandler(this.bntNextFrame_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current Frame";
            // 
            // bntPause
            // 
            this.bntPause.Location = new System.Drawing.Point(355, 8);
            this.bntPause.Name = "bntPause";
            this.bntPause.Size = new System.Drawing.Size(75, 23);
            this.bntPause.TabIndex = 1;
            this.bntPause.Text = "Pause";
            this.bntPause.UseVisualStyleBackColor = true;
            this.bntPause.Click += new System.EventHandler(this.bntPause_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(250, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.bntPlay_Click);
            // 
            // bntWalking
            // 
            this.bntWalking.Location = new System.Drawing.Point(577, 0);
            this.bntWalking.Name = "bntWalking";
            this.bntWalking.Size = new System.Drawing.Size(75, 23);
            this.bntWalking.TabIndex = 1;
            this.bntWalking.Text = "Walking";
            this.bntWalking.UseVisualStyleBackColor = true;
            this.bntWalking.Click += new System.EventHandler(this.bntWalking_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(821, 389);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_Click);
            // 
            // bntDrinking
            // 
            this.bntDrinking.Location = new System.Drawing.Point(658, 0);
            this.bntDrinking.Name = "bntDrinking";
            this.bntDrinking.Size = new System.Drawing.Size(75, 23);
            this.bntDrinking.TabIndex = 6;
            this.bntDrinking.Text = "Drinking";
            this.bntDrinking.UseVisualStyleBackColor = true;
            this.bntDrinking.Click += new System.EventHandler(this.bntDrinking_Click);
            // 
            // FormFrameCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 517);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormFrameCapture";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button bntPause;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bntNextFrame;
        private System.Windows.Forms.Button bntPrevFrame;
        private System.Windows.Forms.Button bntLoadLabels;
        private Button bntWalking;
        private Button bntDrinking;
    }
}

