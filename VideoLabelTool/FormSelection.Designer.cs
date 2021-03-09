﻿
namespace VideoLabelTool
{
    partial class FormSelection
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
            this.listBoxSelection = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bntSelConfirm = new System.Windows.Forms.Button();
            this.bntSelClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxSelection
            // 
            this.listBoxSelection.FormattingEnabled = true;
            this.listBoxSelection.ItemHeight = 12;
            this.listBoxSelection.Location = new System.Drawing.Point(28, 50);
            this.listBoxSelection.Name = "listBoxSelection";
            this.listBoxSelection.Size = new System.Drawing.Size(99, 148);
            this.listBoxSelection.TabIndex = 0;
            this.listBoxSelection.SelectedIndexChanged += new System.EventHandler(this.listBoxSelection_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(401, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select only a person ID which should be associated with the action";
            // 
            // bntSelConfirm
            // 
            this.bntSelConfirm.Location = new System.Drawing.Point(227, 175);
            this.bntSelConfirm.Name = "bntSelConfirm";
            this.bntSelConfirm.Size = new System.Drawing.Size(75, 23);
            this.bntSelConfirm.TabIndex = 2;
            this.bntSelConfirm.Text = "Confirm";
            this.bntSelConfirm.UseVisualStyleBackColor = true;
            this.bntSelConfirm.Click += new System.EventHandler(this.bntSelConfirm_Click);
            // 
            // bntSelClose
            // 
            this.bntSelClose.Location = new System.Drawing.Point(323, 175);
            this.bntSelClose.Name = "bntSelClose";
            this.bntSelClose.Size = new System.Drawing.Size(75, 23);
            this.bntSelClose.TabIndex = 3;
            this.bntSelClose.Text = "Close";
            this.bntSelClose.UseVisualStyleBackColor = true;
            this.bntSelClose.Click += new System.EventHandler(this.bntSelClose_Click);
            // 
            // FormSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 218);
            this.Controls.Add(this.bntSelClose);
            this.Controls.Add(this.bntSelConfirm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxSelection);
            this.Name = "FormSelection";
            this.Text = "Person Selection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxSelection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bntSelConfirm;
        private System.Windows.Forms.Button bntSelClose;
    }
}