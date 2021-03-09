
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.nudEnd = new System.Windows.Forms.NumericUpDown();
            this.nudStart = new System.Windows.Forms.NumericUpDown();
            this.cbInter = new System.Windows.Forms.CheckBox();
            this.bntExport = new System.Windows.Forms.Button();
            this.bntStanding = new System.Windows.Forms.Button();
            this.labelFrame = new System.Windows.Forms.Label();
            this.bntDrinking = new System.Windows.Forms.Button();
            this.bntWalking = new System.Windows.Forms.Button();
            this.bntLoadLabels = new System.Windows.Forms.Button();
            this.bntPrevFrame = new System.Windows.Forms.Button();
            this.bntNextFrame = new System.Windows.Forms.Button();
            this.counterFrame = new System.Windows.Forms.Label();
            this.bntPause = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelTo = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.groupBox);
            this.splitContainer1.Panel1.Controls.Add(this.bntExport);
            this.splitContainer1.Panel1.Controls.Add(this.bntStanding);
            this.splitContainer1.Panel1.Controls.Add(this.labelFrame);
            this.splitContainer1.Panel1.Controls.Add(this.bntDrinking);
            this.splitContainer1.Panel1.Controls.Add(this.bntWalking);
            this.splitContainer1.Panel1.Controls.Add(this.bntLoadLabels);
            this.splitContainer1.Panel1.Controls.Add(this.bntPrevFrame);
            this.splitContainer1.Panel1.Controls.Add(this.bntNextFrame);
            this.splitContainer1.Panel1.Controls.Add(this.counterFrame);
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
            // groupBox
            // 
            this.groupBox.Controls.Add(this.labelTo);
            this.groupBox.Controls.Add(this.nudEnd);
            this.groupBox.Controls.Add(this.nudStart);
            this.groupBox.Controls.Add(this.cbInter);
            this.groupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox.Location = new System.Drawing.Point(12, 20);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(196, 78);
            this.groupBox.TabIndex = 12;
            this.groupBox.TabStop = false;
            // 
            // nudEnd
            // 
            this.nudEnd.Enabled = false;
            this.nudEnd.Location = new System.Drawing.Point(118, 51);
            this.nudEnd.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.nudEnd.Name = "nudEnd";
            this.nudEnd.Size = new System.Drawing.Size(51, 23);
            this.nudEnd.TabIndex = 12;
            this.nudEnd.Visible = false;
            // 
            // nudStart
            // 
            this.nudStart.Enabled = false;
            this.nudStart.Location = new System.Drawing.Point(6, 51);
            this.nudStart.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.nudStart.Name = "nudStart";
            this.nudStart.Size = new System.Drawing.Size(51, 23);
            this.nudStart.TabIndex = 11;
            this.nudStart.Visible = false;
            // 
            // cbInter
            // 
            this.cbInter.AutoSize = true;
            this.cbInter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbInter.Location = new System.Drawing.Point(6, 20);
            this.cbInter.Name = "cbInter";
            this.cbInter.Size = new System.Drawing.Size(128, 19);
            this.cbInter.TabIndex = 10;
            this.cbInter.Text = "Interpolation mode";
            this.cbInter.UseVisualStyleBackColor = true;
            this.cbInter.CheckedChanged += new System.EventHandler(this.cbInter_CheckedChanged);
            // 
            // bntExport
            // 
            this.bntExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntExport.Location = new System.Drawing.Point(459, 52);
            this.bntExport.Name = "bntExport";
            this.bntExport.Size = new System.Drawing.Size(75, 23);
            this.bntExport.TabIndex = 9;
            this.bntExport.Text = "Export";
            this.bntExport.UseVisualStyleBackColor = true;
            this.bntExport.Click += new System.EventHandler(this.bntExport_Click);
            // 
            // bntStanding
            // 
            this.bntStanding.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntStanding.Location = new System.Drawing.Point(664, 0);
            this.bntStanding.Name = "bntStanding";
            this.bntStanding.Size = new System.Drawing.Size(75, 23);
            this.bntStanding.TabIndex = 8;
            this.bntStanding.Text = "Standing";
            this.bntStanding.UseVisualStyleBackColor = true;
            this.bntStanding.Click += new System.EventHandler(this.bntStanding_Click);
            // 
            // labelFrame
            // 
            this.labelFrame.AutoSize = true;
            this.labelFrame.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFrame.Location = new System.Drawing.Point(14, 7);
            this.labelFrame.Name = "labelFrame";
            this.labelFrame.Size = new System.Drawing.Size(40, 15);
            this.labelFrame.TabIndex = 7;
            this.labelFrame.Text = "Frame";
            // 
            // bntDrinking
            // 
            this.bntDrinking.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntDrinking.Location = new System.Drawing.Point(746, 0);
            this.bntDrinking.Name = "bntDrinking";
            this.bntDrinking.Size = new System.Drawing.Size(75, 23);
            this.bntDrinking.TabIndex = 6;
            this.bntDrinking.Text = "Drinking";
            this.bntDrinking.UseVisualStyleBackColor = true;
            this.bntDrinking.Click += new System.EventHandler(this.bntDrinking_Click);
            // 
            // bntWalking
            // 
            this.bntWalking.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntWalking.Location = new System.Drawing.Point(577, 0);
            this.bntWalking.Name = "bntWalking";
            this.bntWalking.Size = new System.Drawing.Size(75, 23);
            this.bntWalking.TabIndex = 1;
            this.bntWalking.Text = "Walking";
            this.bntWalking.UseVisualStyleBackColor = true;
            this.bntWalking.Click += new System.EventHandler(this.bntWalking_Click);
            // 
            // bntLoadLabels
            // 
            this.bntLoadLabels.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntLoadLabels.Location = new System.Drawing.Point(459, 8);
            this.bntLoadLabels.Name = "bntLoadLabels";
            this.bntLoadLabels.Size = new System.Drawing.Size(75, 23);
            this.bntLoadLabels.TabIndex = 2;
            this.bntLoadLabels.Text = "Load";
            this.bntLoadLabels.UseVisualStyleBackColor = true;
            this.bntLoadLabels.Click += new System.EventHandler(this.bntLoadLabels_Click);
            // 
            // bntPrevFrame
            // 
            this.bntPrevFrame.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.bntNextFrame.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntNextFrame.Location = new System.Drawing.Point(355, 53);
            this.bntNextFrame.Name = "bntNextFrame";
            this.bntNextFrame.Size = new System.Drawing.Size(75, 23);
            this.bntNextFrame.TabIndex = 4;
            this.bntNextFrame.Text = "Next F.";
            this.bntNextFrame.UseVisualStyleBackColor = true;
            this.bntNextFrame.Click += new System.EventHandler(this.bntNextFrame_Click);
            // 
            // counterFrame
            // 
            this.counterFrame.AutoSize = true;
            this.counterFrame.Location = new System.Drawing.Point(128, 5);
            this.counterFrame.Name = "counterFrame";
            this.counterFrame.Size = new System.Drawing.Size(0, 12);
            this.counterFrame.TabIndex = 3;
            // 
            // bntPause
            // 
            this.bntPause.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(250, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.bntPlay_Click);
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
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(75, 54);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(19, 15);
            this.labelTo.TabIndex = 13;
            this.labelTo.Text = "To";
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
            this.Text = "Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).EndInit();
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
        private System.Windows.Forms.Label counterFrame;
        private System.Windows.Forms.Button bntNextFrame;
        private System.Windows.Forms.Button bntPrevFrame;
        private System.Windows.Forms.Button bntLoadLabels;
        private Button bntWalking;
        private Button bntDrinking;
        private Label labelFrame;
        private Button bntStanding;
        private Button bntExport;
        private CheckBox cbInter;
        private GroupBox groupBox;
        private NumericUpDown nudEnd;
        private NumericUpDown nudStart;
        private Label labelTo;
    }
}

