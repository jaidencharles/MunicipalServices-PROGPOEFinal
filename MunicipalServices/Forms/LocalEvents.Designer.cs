using System.Drawing;
using System.Windows.Forms;
using MunicipalServices.Utils;

namespace MunicipalServices.Forms
{
    partial class LocalEvents
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (this.BackgroundImage != null)
                {
                    this.BackgroundImage.Dispose();
                    this.BackgroundImage = null;
                }
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lstEvents = new System.Windows.Forms.ListBox();
            this.lblEventDate = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblOrganizer = new System.Windows.Forms.Label();
            this.lblDetails = new System.Windows.Forms.Label();
            this.txtEventDetails = new System.Windows.Forms.TextBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 28F);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(122)))), ((int)(((byte)(183)))));
            this.lblTitle.Location = new System.Drawing.Point(48, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(375, 70);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Local Events";
            // 
            // lstEvents
            // 
            this.lstEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstEvents.ItemHeight = 20;
            this.lstEvents.Location = new System.Drawing.Point(51, 130);
            this.lstEvents.Name = "lstEvents";
            this.lstEvents.Size = new System.Drawing.Size(400, 424);
            this.lstEvents.TabIndex = 1;
            // 
            // lblEventDate
            // 
            this.lblEventDate.Location = new System.Drawing.Point(480, 140);
            this.lblEventDate.Name = "lblEventDate";
            this.lblEventDate.Size = new System.Drawing.Size(400, 30);
            this.lblEventDate.TabIndex = 2;
            this.lblEventDate.Text = "Date:";
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(480, 170);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(400, 30);
            this.lblLocation.TabIndex = 3;
            this.lblLocation.Text = "Location:";
            // 
            // lblOrganizer
            // 
            this.lblOrganizer.Location = new System.Drawing.Point(480, 200);
            this.lblOrganizer.Name = "lblOrganizer";
            this.lblOrganizer.Size = new System.Drawing.Size(400, 30);
            this.lblOrganizer.TabIndex = 4;
            this.lblOrganizer.Text = "Organizer:";
            // 
            // lblDetails
            // 
            this.lblDetails.Location = new System.Drawing.Point(480, 230);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(400, 30);
            this.lblDetails.TabIndex = 5;
            this.lblDetails.Text = "Details:";
            // 
            // txtEventDetails
            // 
            this.txtEventDetails.Location = new System.Drawing.Point(480, 260);
            this.txtEventDetails.Multiline = true;
            this.txtEventDetails.Name = "txtEventDetails";
            this.txtEventDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEventDetails.Size = new System.Drawing.Size(400, 180);
            this.txtEventDetails.TabIndex = 6;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(51, 600);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(120, 40);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "Back";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(137)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(610, 99);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(112, 29);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // LocalEvents
            // 
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(976, 700);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lstEvents);
            this.Controls.Add(this.lblEventDate);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.lblOrganizer);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.txtEventDetails);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LocalEvents";
            this.Text = "Local Events";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lstEvents;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEventDate;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblOrganizer;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.TextBox txtEventDetails;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnSearch;


    }

}
