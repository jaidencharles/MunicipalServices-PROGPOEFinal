using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MunicipalServices.Utils;
using MunicipalServicesLibrary.Data;
using MunicipalServicesLibrary.Models;

namespace MunicipalServices
{
    public partial class ViewReports : Form
    {
        private readonly IssueRepository _issueRepository;
        private Label lblEmptyState;
        private Panel detailPanel;
        private Label lblStatus;

        public ViewReports(IssueRepository issueRepository)
        {
            InitializeComponent();
            _issueRepository = issueRepository;
            InitializeFormStyle();
            SetupInitialState();
            LoadReports();
            btnOpenDocument.Click += btnOpenDocument_Click;
            lstReports.SelectedIndexChanged += lstReports_SelectedIndexChanged;
        }

        private void InitializeFormStyle()
        {
            UiStyle.StyleForm(this);
            Padding = new Padding(40, 28, 40, 28);

            UiStyle.StyleTitle(lblTitle);
            lblTitle.Text = "View reports";
            lblTitle.Location = new Point(40, 28);

            var lblIntro = new Label
            {
                Text = "Select a submitted report to review details and attachments.",
                Location = new Point(40, 72),
                AutoSize = true
            };
            UiStyle.StyleSubtitle(lblIntro);
            Controls.Add(lblIntro);

            lstReports.Location = new Point(40, 120);
            lstReports.Size = new Size(320, 520);
            UiStyle.StyleListBox(lstReports);

            detailPanel = new Panel
            {
                Location = new Point(384, 120),
                Size = new Size(520, 520),
                BackColor = ThemeColors.Surface,
                Padding = new Padding(20)
            };
            UiStyle.StylePanel(detailPanel);

            lblStatus = new Label
            {
                Location = new Point(20, 16),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9F),
                ForeColor = Color.White,
                BackColor = ThemeColors.Info,
                Padding = new Padding(8, 4, 8, 4),
                Visible = false
            };

            lblLocation.Location = new Point(20, 52);
            lblLocation.Size = new Size(460, 28);
            UiStyle.StyleFieldLabel(lblLocation);

            lblCategory.Location = new Point(20, 84);
            lblCategory.Size = new Size(460, 28);
            UiStyle.StyleFieldLabel(lblCategory);

            lblDescription.Location = new Point(20, 124);
            lblDescription.Text = "Description";
            UiStyle.StyleFieldLabel(lblDescription);

            txtDescription.Location = new Point(20, 152);
            txtDescription.Size = new Size(460, 140);
            UiStyle.StyleTextBox(txtDescription);
            txtDescription.Multiline = true;
            txtDescription.ReadOnly = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;

            lblAttachment.Location = new Point(20, 308);
            UiStyle.StyleFieldLabel(lblAttachment);

            pictureBox.Location = new Point(20, 340);
            pictureBox.Size = new Size(460, 140);
            pictureBox.BackColor = ThemeColors.PrimaryLight;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            btnOpenDocument.Location = new Point(20, 340);
            btnOpenDocument.Size = new Size(180, 40);
            btnOpenDocument.Text = "Open document";
            UiStyle.StylePrimaryButton(btnOpenDocument);
            btnOpenDocument.Visible = false;

            detailPanel.Controls.Add(lblStatus);
            detailPanel.Controls.Add(lblLocation);
            detailPanel.Controls.Add(lblCategory);
            detailPanel.Controls.Add(lblDescription);
            detailPanel.Controls.Add(txtDescription);
            detailPanel.Controls.Add(lblAttachment);
            detailPanel.Controls.Add(pictureBox);
            detailPanel.Controls.Add(btnOpenDocument);

            lblEmptyState = new Label
            {
                Text = "No reports yet.\nSubmit an issue to see it listed here.",
                Font = UiStyle.BodyFont,
                ForeColor = ThemeColors.TextSecondary,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Visible = false
            };
            detailPanel.Controls.Add(lblEmptyState);

            Controls.Add(detailPanel);
            Controls.SetChildIndex(detailPanel, 0);
        }

        private void SetupInitialState()
        {
            lblLocation.Text = "Location";
            lblCategory.Text = "Category";
            txtDescription.Text = "Select a report from the list.";
            txtDescription.ForeColor = ThemeColors.TextSecondary;
            lblAttachment.Text = "Attachment";
            lblStatus.Visible = false;
            pictureBox.Visible = false;
            btnOpenDocument.Visible = false;
            lblEmptyState.Visible = false;
        }

        private void LoadReports()
        {
            lstReports.Items.Clear();
            var issues = _issueRepository.GetAllIssues();

            if (issues.Count == 0)
            {
                lstReports.Items.Add("No reports available");
                lstReports.Enabled = false;
                SetupInitialState();
                lblEmptyState.Visible = true;
                lblEmptyState.BringToFront();
            }
            else
            {
                lstReports.Enabled = true;
                lblEmptyState.Visible = false;
                foreach (var issue in issues)
                {
                    lstReports.Items.Add($"{issue.Status}  ·  {issue.Location}  ·  {issue.Category}");
                }
            }
        }

        private void lstReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = lstReports.SelectedIndex;
            var issues = _issueRepository.GetAllIssues();

            if (selectedIndex >= 0 && selectedIndex < issues.Count)
            {
                DisplayIssueDetails(issues[selectedIndex]);
            }
            else
            {
                SetupInitialState();
            }
        }

        private void DisplayIssueDetails(Issue issue)
        {
            if (issue == null) return;

            lblEmptyState.Visible = false;
            lblLocation.ForeColor = ThemeColors.TextPrimary;
            lblCategory.ForeColor = ThemeColors.TextPrimary;
            txtDescription.ForeColor = ThemeColors.TextPrimary;
            lblAttachment.ForeColor = ThemeColors.TextPrimary;

            lblStatus.Text = issue.Status.ToString();
            lblStatus.BackColor = UiStyle.StatusColor(issue.Status.ToString());
            lblStatus.Visible = true;

            lblLocation.Text = "Location: " + issue.Location;
            lblCategory.Text = "Category: " + issue.Category;
            txtDescription.Text = issue.Description;

            if (issue.AttachmentData != null && !string.IsNullOrEmpty(issue.AttachmentName))
            {
                string fileExtension = Path.GetExtension(issue.AttachmentName).ToLowerInvariant();
                lblAttachment.Text = "Attachment: " + issue.AttachmentName;

                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
                {
                    try
                    {
                        using (var ms = new MemoryStream(issue.AttachmentData))
                        {
                            if (pictureBox.Image != null)
                            {
                                pictureBox.Image.Dispose();
                            }
                            pictureBox.Image = Image.FromStream(ms);
                            pictureBox.Visible = true;
                            btnOpenDocument.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        pictureBox.Visible = false;
                        lblAttachment.Text = "Error loading image";
                        MessageBox.Show("Error loading image: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    pictureBox.Visible = false;
                    btnOpenDocument.Visible = true;
                }
            }
            else
            {
                pictureBox.Visible = false;
                btnOpenDocument.Visible = false;
                lblAttachment.Text = "Attachment: none";
            }
        }

        private void btnOpenDocument_Click(object sender, EventArgs e)
        {
            int selectedIndex = lstReports.SelectedIndex;
            var issues = _issueRepository.GetAllIssues();

            if (selectedIndex >= 0 && selectedIndex < issues.Count)
            {
                OpenAttachment(issues[selectedIndex]);
            }
        }

        private void OpenAttachment(Issue issue)
        {
            if (issue.AttachmentData == null || string.IsNullOrEmpty(issue.AttachmentName))
            {
                MessageBox.Show("No attachment available.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string tempPath = Path.Combine(Path.GetTempPath(), issue.AttachmentName);
                File.WriteAllBytes(tempPath, issue.AttachmentData);
                Process.Start(tempPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open attachment: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
