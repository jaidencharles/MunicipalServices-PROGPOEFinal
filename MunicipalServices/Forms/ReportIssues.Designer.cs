using System.Windows.Forms;

namespace MunicipalServices
{
    partial class ReportIssues
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.RichTextBox();
            this.btnAttachFile = new System.Windows.Forms.Button();
            this.lblAttachmentStatus = new System.Windows.Forms.Label();
            this.lblEngagement = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnViewReports = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblEngagementMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(40, 148);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(860, 26);
            this.txtLocation.TabIndex = 1;
            // 
            // cmbCategory
            // 
            this.cmbCategory.Location = new System.Drawing.Point(40, 226);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(860, 28);
            this.cmbCategory.TabIndex = 2;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(40, 304);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(860, 180);
            this.txtDescription.TabIndex = 3;
            // 
            // btnAttachFile
            // 
            this.btnAttachFile.Location = new System.Drawing.Point(40, 530);
            this.btnAttachFile.Name = "btnAttachFile";
            this.btnAttachFile.Size = new System.Drawing.Size(200, 42);
            this.btnAttachFile.TabIndex = 4;
            this.btnAttachFile.Text = "Attach file";
            // 
            // lblAttachmentStatus
            // 
            this.lblAttachmentStatus.Location = new System.Drawing.Point(256, 538);
            this.lblAttachmentStatus.Name = "lblAttachmentStatus";
            this.lblAttachmentStatus.Size = new System.Drawing.Size(640, 28);
            this.lblAttachmentStatus.TabIndex = 5;
            this.lblAttachmentStatus.Text = "No file attached";
            // 
            // lblEngagement
            // 
            this.lblEngagement.Location = new System.Drawing.Point(0, 0);
            this.lblEngagement.Name = "lblEngagement";
            this.lblEngagement.Size = new System.Drawing.Size(100, 23);
            this.lblEngagement.Visible = false;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(40, 668);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(180, 44);
            this.btnSubmit.TabIndex = 8;
            this.btnSubmit.Text = "Submit report";
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(700, 668);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(120, 44);
            this.btnBack.TabIndex = 10;
            this.btnBack.Text = "Back";
            this.btnBack.Visible = false;
            // 
            // btnViewReports
            // 
            this.btnViewReports.Location = new System.Drawing.Point(236, 668);
            this.btnViewReports.Name = "btnViewReports";
            this.btnViewReports.Size = new System.Drawing.Size(160, 44);
            this.btnViewReports.TabIndex = 9;
            this.btnViewReports.Text = "View reports";
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(40, 28);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(860, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Report an issue";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(40, 600);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(860, 8);
            this.progressBar.TabIndex = 6;
            // 
            // lblEngagementMessage
            // 
            this.lblEngagementMessage.Location = new System.Drawing.Point(40, 618);
            this.lblEngagementMessage.Name = "lblEngagementMessage";
            this.lblEngagementMessage.Size = new System.Drawing.Size(860, 28);
            this.lblEngagementMessage.TabIndex = 7;
            this.lblEngagementMessage.Text = "0 of 4 complete";
            // 
            // ReportIssues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(960, 760);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.btnAttachFile);
            this.Controls.Add(this.lblAttachmentStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblEngagementMessage);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnViewReports);
            this.Controls.Add(this.btnBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ReportIssues";
            this.Text = "Report an issue";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.RichTextBox txtDescription;
        private System.Windows.Forms.Button btnAttachFile;
        private System.Windows.Forms.Label lblAttachmentStatus;
        private System.Windows.Forms.Label lblEngagement;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnViewReports;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblEngagementMessage;
    }
}
