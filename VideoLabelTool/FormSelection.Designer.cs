
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
            this.SuspendLayout();
            // 
            // listBoxSelection
            // 
            this.listBoxSelection.FormattingEnabled = true;
            this.listBoxSelection.ItemHeight = 12;
            this.listBoxSelection.Location = new System.Drawing.Point(211, 44);
            this.listBoxSelection.Name = "listBoxSelection";
            this.listBoxSelection.Size = new System.Drawing.Size(132, 136);
            this.listBoxSelection.TabIndex = 0;
            this.listBoxSelection.SelectedIndexChanged += new System.EventHandler(this.listBoxSelection_SelectedIndexChanged);
            // 
            // FormSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 210);
            this.Controls.Add(this.listBoxSelection);
            this.Name = "FormSelection";
            this.Text = "Person Selection";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxSelection;
    }
}