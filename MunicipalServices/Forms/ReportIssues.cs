using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MunicipalServices.Utils;
using MunicipalServicesLibrary.Data;
using MunicipalServicesLibrary.Models;

namespace MunicipalServices
{
    public partial class ReportIssues : Form
    {
        private readonly IssueRepository _issueRepository;
        private byte[] attachmentData;
        private string attachmentName;
        private string attachmentType;

        private Label lblLocationField;
        private Label lblCategoryField;
        private Label lblDescriptionField;
        private Label lblAttachmentField;
        private const string LocationPlaceholder = "Enter the street, landmark, or area";
        private const string DescriptionPlaceholder = "Describe the issue and anything crews should know...";

        public ReportIssues()
        {
            InitializeComponent();
            _issueRepository = new IssueRepository();
            InitializeFormStyle();
            SetupEventHandlers();
            UpdateProgress(null, null);
        }

        private void InitializeFormStyle()
        {
            UiStyle.StyleForm(this);
            Padding = new Padding(40, 28, 40, 28);

            UiStyle.StyleTitle(lblTitle);
            lblTitle.Text = "Report an issue";
            lblTitle.Location = new Point(40, 28);

            var lblIntro = new Label
            {
                Text = "Share what needs attention. Fields marked complete update the progress bar below.",
                Location = new Point(40, 72),
                MaximumSize = new Size(860, 0),
                AutoSize = true
            };
            UiStyle.StyleSubtitle(lblIntro);
            if (!Controls.Contains(lblIntro))
                Controls.Add(lblIntro);

            lblLocationField = EnsureFieldLabel("Location", 120);
            txtLocation.Location = new Point(40, 148);
            txtLocation.Size = new Size(860, 32);
            UiStyle.StylePlaceholder(txtLocation, LocationPlaceholder);

            lblCategoryField = EnsureFieldLabel("Category", 198);
            cmbCategory.Location = new Point(40, 226);
            cmbCategory.Size = new Size(860, 32);
            cmbCategory.Items.Clear();
            cmbCategory.Items.AddRange(new object[]
            {
                "Infrastructure",
                "Public Safety",
                "Environmental",
                "Utilities",
                "Transportation",
                "Parks and Recreation",
                "Public Health",
                "Other"
            });
            cmbCategory.SelectedIndex = -1;
            UiStyle.StyleComboBox(cmbCategory);

            lblDescriptionField = EnsureFieldLabel("Description", 276);
            txtDescription.Location = new Point(40, 304);
            txtDescription.Size = new Size(860, 180);
            UiStyle.StyleRichTextBox(txtDescription, DescriptionPlaceholder);

            lblAttachmentField = EnsureFieldLabel("Attachment (optional)", 502);
            btnAttachFile.Location = new Point(40, 530);
            btnAttachFile.Size = new Size(200, 42);
            btnAttachFile.Text = "Attach file";
            UiStyle.StyleSecondaryButton(btnAttachFile);

            lblAttachmentStatus.Location = new Point(256, 538);
            lblAttachmentStatus.AutoSize = true;
            lblAttachmentStatus.Font = UiStyle.BodyFont;
            lblAttachmentStatus.ForeColor = ThemeColors.TextSecondary;
            lblAttachmentStatus.Text = "No file attached";

            progressBar.Location = new Point(40, 600);
            progressBar.Size = new Size(860, 8);
            progressBar.Style = ProgressBarStyle.Continuous;

            lblEngagementMessage.Location = new Point(40, 618);
            lblEngagementMessage.AutoSize = true;
            lblEngagementMessage.Font = UiStyle.BodyFont;
            lblEngagementMessage.ForeColor = ThemeColors.Primary;

            btnSubmit.Location = new Point(40, 668);
            btnSubmit.Size = new Size(180, 44);
            btnSubmit.Text = "Submit report";
            UiStyle.StyleAccentButton(btnSubmit);

            btnViewReports.Location = new Point(236, 668);
            btnViewReports.Size = new Size(160, 44);
            btnViewReports.Text = "View reports";
            UiStyle.StyleSecondaryButton(btnViewReports);

            btnBack.Visible = false;
        }

        private Label EnsureFieldLabel(string text, int y)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(40, y),
                AutoSize = true
            };
            UiStyle.StyleFieldLabel(label);
            Controls.Add(label);
            label.BringToFront();
            return label;
        }

        private void SetupEventHandlers()
        {
            txtLocation.Leave += (s, e) => UpdateProgress(s, e);
            txtDescription.TextChanged += (s, e) => UpdateProgress(s, e);
            cmbCategory.SelectedIndexChanged += (s, e) => UpdateProgress(s, e);
            btnSubmit.Click += btnSubmit_Click;
            btnViewReports.Click += btnViewReports_Click;
            btnAttachFile.Click += btnAttachFile_Click;
        }

        private void UpdateProgress(object sender, EventArgs e)
        {
            int filled = 0;

            if (!string.IsNullOrWhiteSpace(txtLocation.Text) && txtLocation.Text != LocationPlaceholder)
                filled++;

            if (cmbCategory.SelectedIndex >= 0)
                filled++;

            if (!string.IsNullOrWhiteSpace(txtDescription.Text) && txtDescription.Text != DescriptionPlaceholder)
                filled++;

            if (!string.IsNullOrEmpty(attachmentName))
                filled++;

            progressBar.Value = filled * 25;

            string completeness = filled + " of 4 complete";
            switch (filled)
            {
                case 0:
                    lblEngagementMessage.Text = completeness + " — let's get started.";
                    break;
                case 1:
                    lblEngagementMessage.Text = completeness + " — good start.";
                    break;
                case 2:
                    lblEngagementMessage.Text = completeness + " — halfway there.";
                    break;
                case 3:
                    lblEngagementMessage.Text = completeness + " — almost ready.";
                    break;
                default:
                    lblEngagementMessage.Text = completeness + " — ready to submit.";
                    break;
            }
        }

        private void btnViewReports_Click(object sender, EventArgs e)
        {
            var shell = FindForm() as Form1;
            if (shell != null)
            {
                shell.ShowScreen(AppScreen.ViewReports);
            }
        }

        private void btnAttachFile_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Image and Document files|*.jpg;*.jpeg;*.png;*.pdf;*.docx|All files|*.*";
                fileDialog.Title = "Select an Image or Document";

                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    attachmentData = File.ReadAllBytes(fileDialog.FileName);
                    attachmentName = Path.GetFileName(fileDialog.FileName);
                    attachmentType = Path.GetExtension(fileDialog.FileName).ToLowerInvariant();

                    if (attachmentType == ".jpg" || attachmentType == ".jpeg" || attachmentType == ".png"
                        || attachmentType == ".pdf" || attachmentType == ".docx")
                    {
                        lblAttachmentStatus.Text = "Attached: " + attachmentName;
                        lblAttachmentStatus.ForeColor = ThemeColors.Success;
                    }
                    else
                    {
                        lblAttachmentStatus.Text = "Unsupported file type. Please attach an image or document.";
                        lblAttachmentStatus.ForeColor = ThemeColors.Danger;
                        ClearAttachment();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading file: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ClearAttachment();
                }
            }

            UpdateProgress(null, null);
        }

        private void ClearAttachment()
        {
            attachmentData = null;
            attachmentName = null;
            attachmentType = null;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string location = txtLocation.Text;
            string category = cmbCategory.SelectedItem?.ToString();
            string description = txtDescription.Text;

            if (string.IsNullOrWhiteSpace(location) || location == LocationPlaceholder
                || string.IsNullOrWhiteSpace(category)
                || string.IsNullOrWhiteSpace(description) || description == DescriptionPlaceholder)
            {
                MessageBox.Show("Please fill in location, category, and description before submitting.",
                    "Missing information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newIssue = new Issue
            {
                Location = location,
                Category = category,
                Description = description,
                ReportDate = DateTime.Now,
                Status = IssueStatus.Reported,
                AttachmentName = attachmentName,
                AttachmentType = attachmentType,
                AttachmentData = attachmentData
            };

            _issueRepository.AddIssue(newIssue);

            MessageBox.Show("Your report has been successfully submitted!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtLocation.Text = LocationPlaceholder;
            txtLocation.ForeColor = ThemeColors.TextSecondary;
            cmbCategory.SelectedIndex = -1;
            txtDescription.Text = DescriptionPlaceholder;
            txtDescription.ForeColor = ThemeColors.TextSecondary;
            lblAttachmentStatus.Text = "No file attached";
            lblAttachmentStatus.ForeColor = ThemeColors.TextSecondary;
            ClearAttachment();
            UpdateProgress(null, null);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var shell = FindForm() as Form1;
            if (shell != null)
            {
                shell.ShowScreen(AppScreen.Home);
            }
        }
    }
}
