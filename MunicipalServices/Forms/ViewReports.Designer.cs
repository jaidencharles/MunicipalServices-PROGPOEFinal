using System.Windows.Forms;
using MunicipalServices.Utils;

namespace MunicipalServices
{
    partial class ViewReports
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
            this.lstReports = new System.Windows.Forms.ListBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.lblAttachment = new System.Windows.Forms.Label();
            this.btnOpenDocument = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // lstReports
            // 
            this.lstReports.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstReports.ItemHeight = 20;
            this.lstReports.Location = new System.Drawing.Point(40, 100);
            this.lstReports.Name = "lstReports";
            this.lstReports.Size = new System.Drawing.Size(300, 584);
            this.lstReports.TabIndex = 1;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(380, 100);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(580, 30);
            this.lblLocation.TabIndex = 2;
            // 
            // lblCategory
            // 
            this.lblCategory.Location = new System.Drawing.Point(380, 140);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(580, 30);
            this.lblCategory.TabIndex = 3;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(380, 180);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(580, 30);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(380, 220);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(580, 150);
            this.txtDescription.TabIndex = 5;
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.Location = new System.Drawing.Point(380, 430);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(580, 270);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 7;
            this.pictureBox.TabStop = false;
            // 
            // lblAttachment
            // 
            this.lblAttachment.Location = new System.Drawing.Point(380, 390);
            this.lblAttachment.Name = "lblAttachment";
            this.lblAttachment.Size = new System.Drawing.Size(580, 30);
            this.lblAttachment.TabIndex = 6;
            this.lblAttachment.Text = "Attachment:";
            // 
            // btnOpenDocument
            // 
            this.btnOpenDocument.Location = new System.Drawing.Point(380, 430);
            this.btnOpenDocument.Name = "btnOpenDocument";
            this.btnOpenDocument.Size = new System.Drawing.Size(200, 45);
            this.btnOpenDocument.TabIndex = 8;
            this.btnOpenDocument.Text = "Open Document";
            this.btnOpenDocument.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(40, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(920, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "View Reports";
            // 
            // ViewReports
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1000, 800);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lstReports);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblAttachment);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btnOpenDocument);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ViewReports";
            this.Padding = new System.Windows.Forms.Padding(40);
            this.Text = "View Reports - Helping Hands Connect";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lstReports;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label lblAttachment;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnOpenDocument;
        private System.Windows.Forms.Label lblTitle;
    }
}