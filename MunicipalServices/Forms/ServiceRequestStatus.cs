using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using MunicipalServicesLibrary.Models;
using MunicipalServicesLibrary.Data;
using MunicipalServices.Utils;
using MunicipalServicesLibrary.DataStructures;
using MunicipalServicesLibrary.Services;

namespace MunicipalServices.Forms
{
    public partial class ServiceRequestStatus : Form
    {
        private readonly ServiceRequestGraph _requestGraph;
        private readonly RedBlackTree<ServiceRequest> _requestTree;
        private Panel graphPanel;
        private readonly IssueRepository _issueRepository;
        private readonly StatusManagementService _statusService;
        private CheckBox chkShowGraph;
        private bool _showGraph;

        public ServiceRequestStatus()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            _requestGraph = new ServiceRequestGraph();
            _requestTree = new RedBlackTree<ServiceRequest>();
            _issueRepository = new IssueRepository();
            _statusService = new StatusManagementService(_issueRepository, _requestGraph);
            
            // Subscribe to status changes
            _statusService.OnStatusChanged += (requestId, status) => 
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateUI()));
                }
                else
                {
                    UpdateUI();
                }
            };
            
            InitializeFormStyle();
            LoadDummyData();
            SetupEventHandlers();
            SimulateStatusUpdates();
        }

        private void InitializeFormStyle()
        {
            UiStyle.StyleForm(this);
            this.AutoScroll = true;
            this.MinimumSize = new Size(900, 700);
            this.Size = new Size(960, 780);
            this.Padding = new Padding(40, 28, 40, 28);
            
            // Title
            lblTitle.Location = new Point(40, 28);
            lblTitle.Text = "Request status";
            UiStyle.StyleTitle(lblTitle);
            
            // Search panel
            Panel searchPanel = new Panel
            {
                Location = new Point(40, 80),
                Size = new Size(400, 40)
            };
            
            txtSearch.Size = new Size(300, 40);
            txtSearch.Location = new Point(0, 0);
            btnSearch.Size = new Size(90, 40);
            btnSearch.Location = new Point(310, 0);
            
            searchPanel.Controls.AddRange(new Control[] { txtSearch, btnSearch });
            
            // Request list
            lstRequests.Location = new Point(40, 140);
            lstRequests.Size = new Size(400, 300);
            
            // Details panel
            pnlDetails.Location = new Point(468, 140);
            pnlDetails.Size = new Size(422, 300);
            UiStyle.StylePanel(pnlDetails);
            
            chkShowGraph = new CheckBox
            {
                Text = "Show relationships",
                Location = new Point(40, 456),
                AutoSize = true,
                Font = UiStyle.BodyFont,
                ForeColor = ThemeColors.TextPrimary,
                Checked = false
            };
            chkShowGraph.CheckedChanged += (s, e) =>
            {
                _showGraph = chkShowGraph.Checked;
                if (graphPanel != null)
                {
                    graphPanel.Visible = _showGraph;
                    if (_showGraph)
                        graphPanel.Invalidate();
                }
            };

            StyleSearchControls();
            StyleListBoxes();
            StyleDetailsPanel();
            DrawRequestGraph();
            if (graphPanel != null)
                graphPanel.Visible = false;
            
            // Add controls in correct order (graphPanel created by DrawRequestGraph)
            this.Controls.Clear();
            this.Controls.Add(lblTitle);
            this.Controls.Add(searchPanel);
            this.Controls.Add(lstRequests);
            this.Controls.Add(pnlDetails);
            this.Controls.Add(chkShowGraph);
            if (graphPanel != null)
                this.Controls.Add(graphPanel);
            
            // Add selection change event handler
            lstRequests.SelectedIndexChanged += (s, e) =>
            {
                if (lstRequests.SelectedItem != null)
                {
                    string selectedRequestId = lstRequests.SelectedItem.ToString().Split('-')[0].Trim();
                    DisplayRequestDetails(selectedRequestId);
                    UpdateRelatedRequests(selectedRequestId);
                    UpdateGraphDisplay(selectedRequestId);
                }
            };
            
            ShowEmptyState();
            ShowEmptyGraph();
        }

        private void StyleSearchControls()
        {
            UiStyle.StylePlaceholder(txtSearch, "Enter Request ID");
            txtSearch.Size = new Size(300, 40);
            UiStyle.StylePrimaryButton(btnSearch);
            btnSearch.Text = "Search";
            btnSearch.Size = new Size(90, 40);
        }

        private void StyleListBoxes()
        {
            UiStyle.StyleListBox(lstRequests);
            UiStyle.StyleListBox(lstRelatedRequests);
        }

        private void StyleDetailsPanel()
        {
            UiStyle.StylePanel(pnlDetails);
            pnlDetails.BorderStyle = BorderStyle.FixedSingle;
            
            lblRequestId.AutoSize = true;
            lblStatus.AutoSize = true;
            lblDescription.AutoSize = true;
            lblRelatedRequests.AutoSize = true;
            
            lblRequestId.Location = new Point(15, 15);
            lblStatus.Location = new Point(15, 45);
            lblDescription.Location = new Point(15, 75);
            txtDescription.Location = new Point(15, 105);
            txtDescription.Size = new Size(pnlDetails.Width - 30, 80);
            progressBar.Location = new Point(15, 195);
            progressBar.Size = new Size(pnlDetails.Width - 30, 20);
            lblRelatedRequests.Location = new Point(15, 225);
            lstRelatedRequests.Location = new Point(15, 255);
            lstRelatedRequests.Size = new Size(pnlDetails.Width - 30, 80);
            
            lblRelatedRequests.Text = "Related requests";
            lblDescription.Text = "Description";
            
            UiStyle.StyleFieldLabel(lblRequestId);
            UiStyle.StyleFieldLabel(lblStatus);
            UiStyle.StyleFieldLabel(lblDescription);
            UiStyle.StyleFieldLabel(lblRelatedRequests);
            txtDescription.Font = UiStyle.BodyFont;
            txtDescription.ReadOnly = true;
            txtDescription.BorderStyle = BorderStyle.None;
            txtDescription.BackColor = ThemeColors.PrimaryLight;
            
            progressBar.Style = ProgressBarStyle.Continuous;
            
            pnlDetails.Controls.Clear();
            pnlDetails.Controls.AddRange(new Control[] {
                lblRequestId,
                lblStatus,
                lblDescription,
                txtDescription,
                progressBar,
                lblRelatedRequests,
                lstRelatedRequests
            });
        }

        private void StyleButton(Button button, Color color)
        {
            if (color == ThemeColors.Primary || color == ThemeColors.Accent)
                UiStyle.StylePrimaryButton(button);
            else
                UiStyle.StyleMutedButton(button);
        }

        private void SetupEventHandlers()
        {
            btnSearch.Click += btnSearch_Click;
            lstRequests.SelectedIndexChanged += lstRequests_SelectedIndexChanged;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Enter Request ID")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = ThemeColors.TextPrimary;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Enter Request ID";
                txtSearch.ForeColor = ThemeColors.TextSecondary;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchId = txtSearch.Text.Trim();
            if (searchId == "Enter Request ID" || string.IsNullOrEmpty(searchId))
            {
                MessageBox.Show("Please enter a Request ID", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Find the request in the list
            var matchingItem = lstRequests.Items.Cast<string>()
                .FirstOrDefault(item => item.StartsWith(searchId, StringComparison.OrdinalIgnoreCase));

            if (matchingItem != null)
            {
                lstRequests.SelectedItem = matchingItem;
            }
            else
            {
                MessageBox.Show("Request ID not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lstRequests_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstRequests.SelectedItem == null) return;

            string selectedText = lstRequests.SelectedItem.ToString();
            string requestId = selectedText.Split('-')[0].Trim();

            DisplayRequestDetails(requestId);
            UpdateRelatedRequests(requestId);
            graphPanel?.Invalidate();
        }

        private void DisplayRequestDetails(string requestId)
        {
            if (string.IsNullOrEmpty(requestId))
            {
                ShowEmptyState();
                return;
            }

            var issue = _issueRepository.GetAllIssues()
                .FirstOrDefault(i => i.Id.ToString("X8") == requestId);

            if (issue == null)
            {
                ShowEmptyState();
                return;
            }
            
            pnlDetails.Controls.Clear();

            // Request ID
            var lblRequestId = new Label
            {
                Text = $"Request ID: {requestId}",
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            // Status
            var lblStatus = new Label
            {
                Text = $"Status: {issue.Status}",
                Location = new Point(15, 45),
                AutoSize = true
            };

            // Description
            var lblDescTitle = new Label
            {
                Text = "Description:",
                Location = new Point(15, 75),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            var txtDescription = new TextBox
            {
                Text = issue.Description,
                Location = new Point(15, 95),
                Size = new Size(380, 60),
                Multiline = true,
                ReadOnly = true
            };

            // Progress Bar
            var progressBar = new ProgressBar
            {
                Location = new Point(15, 165),
                Size = new Size(380, 23),
                Style = ProgressBarStyle.Continuous,
                Value = 0,
                Maximum = 100
            };

            // Add controls first
            pnlDetails.Controls.AddRange(new Control[] 
            { 
                lblRequestId, 
                lblStatus, 
                lblDescTitle, 
                txtDescription,
                progressBar 
            });

            // Then update progress bar
            UpdateProgressBar(issue.Status.ToString(), progressBar);

            // Add status update button
            AddStatusUpdateControls();

            // Update graph
            UpdateGraphDisplay(requestId);
        }

        private void UpdateProgressBar(string status)
        {
            // Find the progress bar in the details panel
            var progressBar = pnlDetails.Controls.OfType<ProgressBar>().FirstOrDefault();
            if (progressBar == null) return;

            int progress = 0;
            switch (status.ToLower())
            {
                case "reported":
                    progress = 25;
                    progressBar.ForeColor = Color.FromArgb(51, 122, 183); // Blue
                    break;
                case "inprogress":
                    progress = 50;
                    progressBar.ForeColor = Color.FromArgb(240, 173, 78); // Orange
                    break;
                case "resolved":
                    progress = 75;
                    progressBar.ForeColor = Color.FromArgb(92, 184, 92); // Green
                    break;
                case "closed":
                    progress = 100;
                    progressBar.ForeColor = Color.FromArgb(217, 83, 79); // Red
                    break;
                default:
                    progress = 0;
                    progressBar.ForeColor = Color.Gray;
                    break;
            }
            progressBar.Value = progress;
        }

        private void UpdateRelatedRequests(string requestId)
        {
            var relatedRequests = _requestGraph.GetRelatedRequests(requestId);
            lstRelatedRequests.Items.Clear();
            foreach (var request in relatedRequests)
            {
                lstRelatedRequests.Items.Add($"{request.RequestId} - {request.Status}");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var shell = FindForm() as global::MunicipalServices.Form1;
            if (shell != null)
            {
                shell.ShowScreen(global::MunicipalServices.AppScreen.Home);
            }
        }

        private void LoadDummyData()
        {
            var issues = _issueRepository.GetAllIssues();
            var issuesList = issues.ToList();

            for (int i = 0; i < issuesList.Count; i++)
            {
                var issue = issuesList[i];
                var request = new ServiceRequest 
                { 
                    RequestId = issue.Id.ToString("X8"),
                    Description = issue.Description,
                    Category = issue.Category,
                    Priority = DeterminePriority(issue.Category),
                    Status = GetCurrentDetailedStatus(issue.Status).ToString()
                };

                _requestTree.Insert(request);
                _requestGraph.AddRequest(request.RequestId, request.Status);
                lstRequests.Items.Add($"{request.RequestId} - {request.Description} ({request.Status})");

                // Initialize status in status management service
                _statusService.InitializeStatus(request.RequestId, GetCurrentDetailedStatus(issue.Status));

                if (i > 0)
                {
                    _requestGraph.AddRelatedRequests(request.RequestId, issuesList[i - 1].Id.ToString("X8"));
                }
            }
        }

        private void DrawRequestGraph()
        {
            graphPanel = new Panel
            {
                Location = new Point(40, 490),
                Size = new Size(850, 260),
                BackColor = ThemeColors.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = _showGraph
            };

            graphPanel.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                if (!_showGraph)
                    return;

                if (lstRequests.SelectedItem != null)
                {
                    string selectedId = lstRequests.SelectedItem.ToString().Split('-')[0].Trim();
                    var relatedRequests = _requestGraph.GetRelatedRequests(selectedId).ToList();

                    // Draw center node (selected request)
                    int centerX = graphPanel.Width / 2;
                    int centerY = graphPanel.Height / 2;
                    
                    using (var brush = new SolidBrush(ThemeColors.Primary))
                    using (var pen = new Pen(Color.White, 2))
                    {
                        g.FillEllipse(brush, centerX - 40, centerY - 40, 80, 80);
                        g.DrawString(selectedId.Substring(0, Math.Min(4, selectedId.Length)), 
                            new Font("Segoe UI", 10), 
                            Brushes.White, 
                            new RectangleF(centerX - 30, centerY - 10, 60, 20),
                            new StringFormat { Alignment = StringAlignment.Center });
                    }

                    // Draw related nodes
                    if (relatedRequests.Any())
                    {
                        double angleStep = Math.PI * 2 / relatedRequests.Count;
                        for (int i = 0; i < relatedRequests.Count; i++)
                        {
                            double angle = i * angleStep;
                            int x = centerX + (int)(150 * Math.Cos(angle));
                            int y = centerY + (int)(150 * Math.Sin(angle));
                            
                            using (var pen = new Pen(ThemeColors.Primary, 2))
                            {
                                g.DrawLine(pen, centerX, centerY, x, y);
                            }
                            
                            using (var brush = new SolidBrush(UiStyle.StatusColor(relatedRequests[i].Status)))
                            {
                                g.FillEllipse(brush, x - 30, y - 30, 60, 60);
                                g.DrawString(relatedRequests[i].RequestId.Substring(0, Math.Min(4, relatedRequests[i].RequestId.Length)),
                                    new Font("Segoe UI", 9),
                                    Brushes.White,
                                    new RectangleF(x - 25, y - 10, 50, 20),
                                    new StringFormat { Alignment = StringAlignment.Center });
                            }
                        }
                    }
                }
            };

            this.Controls.Add(graphPanel);
        }

        private Color GetStatusColor(string status)
        {
            return UiStyle.StatusColor(status);
        }

        private void SimulateStatusUpdates()
        {
            var statusTimer = new System.Windows.Forms.Timer();
            statusTimer.Interval = 15000; // 15 seconds
            
            statusTimer.Tick += (s, e) =>
            {
                bool anyUpdates = false;
                foreach (var issue in _issueRepository.GetAllIssues())
                {
                    if (issue.Status != IssueStatus.Closed)
                    {
                        var currentStatus = GetCurrentDetailedStatus(issue.Status);
                        var nextStatus = GetNextDetailedStatus(currentStatus);
                        
                        if (nextStatus != currentStatus)
                        {
                            issue.Status = MapToIssueStatus(nextStatus);
                            anyUpdates = true;
                            
                            // Update graph
                            _requestGraph.UpdateRequestStatus(
                                issue.Id.ToString("X8"), 
                                nextStatus.ToString()
                            );
                        }
                    }
                }
                
                if (anyUpdates)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => UpdateUI()));
                    }
                    else
                    {
                        UpdateUI();
                    }
                }
            };
            
            statusTimer.Start();
        }

        private RequestStatus GetCurrentDetailedStatus(IssueStatus status)
        {
            switch (status)
            {
                case IssueStatus.Reported:
                    return RequestStatus.Reported;
                case IssueStatus.InProgress:
                    return RequestStatus.InProgress;
                case IssueStatus.Resolved:
                    return RequestStatus.Resolved;
                case IssueStatus.Closed:
                    return RequestStatus.Closed;
                default:
                    return RequestStatus.Reported;
            }
        }

        private RequestStatus GetNextDetailedStatus(RequestStatus currentStatus)
        {
            switch (currentStatus)
            {
                case RequestStatus.Reported:
                    return RequestStatus.InReview;
                case RequestStatus.InReview:
                    return RequestStatus.Assigned;
                case RequestStatus.Assigned:
                    return RequestStatus.InProgress;
                case RequestStatus.InProgress:
                    return RequestStatus.Resolved;
                case RequestStatus.Resolved:
                    return RequestStatus.Closed;
                case RequestStatus.Closed:
                    return RequestStatus.Closed;
                default:
                    return RequestStatus.Reported;
            }
        }

        private void UpdateUI()
        {
            RefreshRequestsList();
            
            if (lstRequests.SelectedItem != null)
            {
                string selectedRequestId = lstRequests.SelectedItem.ToString().Split('-')[0].Trim();
                var issue = _issueRepository.GetAllIssues()
                    .FirstOrDefault(i => i.Id.ToString("X8") == selectedRequestId);
                    
                if (issue != null)
                {
                    // Update the request tree
                    var request = new ServiceRequest
                    {
                        RequestId = selectedRequestId,
                        Description = issue.Description,
                        Status = issue.Status.ToString()
                    };
                    _requestTree.Insert(request);

                    // Update display
                    lblRequestId.Text = $"Request ID: {selectedRequestId}";
                    lblStatus.Text = $"Status: {issue.Status}";
                    txtDescription.Text = issue.Description;
                    
                    // Update progress bar
                    var progressBar = pnlDetails.Controls.OfType<ProgressBar>().FirstOrDefault();
                    if (progressBar == null)
                    {
                        progressBar = new ProgressBar
                        {
                            Location = new Point(15, 165),
                            Size = new Size(380, 23)
                        };
                        pnlDetails.Controls.Add(progressBar);
                    }
                    UpdateProgressBar(issue.Status.ToString(), progressBar);
                    
                    UpdateRelatedRequests(selectedRequestId);
                    UpdateGraphDisplay(selectedRequestId);
                }
            }
        }

        private void UpdateProgressBar(string status, ProgressBar progressBar)
        {
            
            int progress = 0;
            switch (status.ToLower())
            {
                case "reported":
                    progress = 25;
                    progressBar.ForeColor = Color.FromArgb(51, 122, 183); // Blue
                    break;
                case "inprogress":
                    progress = 50;
                    progressBar.ForeColor = Color.FromArgb(240, 173, 78); // Orange
                    break;
                case "resolved":
                    progress = 75;
                    progressBar.ForeColor = Color.FromArgb(92, 184, 92); // Green
                    break;
                case "closed":
                    progress = 100;
                    progressBar.ForeColor = Color.FromArgb(217, 83, 79); // Red
                    break;
                default:
                    progress = 0;
                    progressBar.ForeColor = Color.Gray;
                    break;
            }
            progressBar.Value = progress;
        }

        private IssueStatus MapToIssueStatus(RequestStatus status)
        {
            switch (status)
            {
                case RequestStatus.Reported:
                    return IssueStatus.Reported;
                case RequestStatus.InReview:
                case RequestStatus.Assigned:
                case RequestStatus.InProgress:
                    return IssueStatus.InProgress;
                case RequestStatus.Resolved:
                    return IssueStatus.Resolved;
                case RequestStatus.Closed:
                    return IssueStatus.Closed;
                default:
                    return IssueStatus.Reported;
            }
        }

        private void RefreshRequestsList()
        {
            var selectedIndex = lstRequests.SelectedIndex;
            lstRequests.Items.Clear();
            
            var issues = _issueRepository.GetAllIssues();
            foreach (var issue in issues)
            {
                // Map the status consistently
                var detailedStatus = GetCurrentDetailedStatus(issue.Status);
                lstRequests.Items.Add($"{issue.Id:X8} - {issue.Description} ({detailedStatus})");
            }
            
            if (selectedIndex >= 0 && selectedIndex < lstRequests.Items.Count)
            {
                lstRequests.SelectedIndex = selectedIndex;
                string selectedRequestId = lstRequests.SelectedItem.ToString().Split('-')[0].Trim();
                DisplayRequestDetails(selectedRequestId);
                UpdateRelatedRequests(selectedRequestId);
            }
        }

        // Event handlers and other functionality will be continued...

        public void SetAutoScaleMode(AutoScaleMode mode)
        {
            this.AutoScaleMode = mode;
        }

        private void UpdateRequestStatus(string requestId, RequestStatus newStatus)
        {
            _statusService.UpdateStatus(requestId, newStatus);
            RefreshRequestsList();
            DisplayRequestDetails(requestId);
            UpdateProgressBar(newStatus.ToString());
            UpdateGraphDisplay(requestId);
        }

        private void AddStatusUpdateControls()
        {
            var btnUpdateStatus = new Button
            {
                Text = "Update Status",
                Location = new Point(15, 195),
                Size = new Size(120, 30)
            };
            StyleButton(btnUpdateStatus, ThemeColors.Primary);

            btnUpdateStatus.Click += (s, e) =>
            {
                if (lstRequests.SelectedItem == null) return;

                var statusForm = new StatusUpdateForm();
                if (statusForm.ShowDialog() == DialogResult.OK)
                {
                    string requestId = lstRequests.SelectedItem.ToString().Split('-')[0].Trim();
                    UpdateRequestStatus(requestId, statusForm.SelectedStatus);
                }
            };

            pnlDetails.Controls.Add(btnUpdateStatus);
        }

        private int DeterminePriority(string category)
        {
            switch (category.ToLower())
            {
                // Priority 1 (Highest) - Immediate safety and health concerns
                case "public safety":
                case "public health":
                    return 1;

                // Priority 2 - Essential services
                case "infrastructure":
                case "utilities":
                    return 2;

                // Priority 3 - Transportation and environmental issues
                case "transportation":
                case "environmental":
                    return 3;

                // Priority 4 - Quality of life services
                case "parks and recreation":
                    return 4;

                // Priority 5 (Lowest) - Miscellaneous
                case "other":
                default:
                    return 5;
            }
        }

        private void UpdateGraphDisplay(string requestId)
        {
            if (graphPanel == null)
            {
                graphPanel = new Panel
                {
                    Location = new Point(48, 460),
                    Size = new Size(854, 300),
                    BackColor = Color.White
                };
                this.Controls.Add(graphPanel);
            }

            graphPanel.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Get all requests with same status or related requests
                var allRequests = _issueRepository.GetAllIssues()
                    .Select(i => new { Id = i.Id.ToString("X8"), Status = i.Status })
                    .ToList();
                    
                var selectedRequest = allRequests.FirstOrDefault(r => r.Id == requestId);
                if (selectedRequest == null) return;

                var centerX = graphPanel.Width / 2;
                var centerY = graphPanel.Height / 2;

                // Draw main node
                DrawNode(g, centerX, centerY, requestId, Color.RoyalBlue);

                // Draw related nodes in a circle
                var radius = 120;
                var count = allRequests.Count - 1; // Exclude selected request
                if (count > 0)
                {
                    var angle = Math.PI * 2 / count;
                    var index = 0;

                    foreach (var request in allRequests.Where(r => r.Id != requestId))
                    {
                        var x = centerX + radius * Math.Cos(angle * index);
                        var y = centerY + radius * Math.Sin(angle * index);
                        
                        // Draw connection line with color based on status relationship
                        var lineColor = request.Status == selectedRequest.Status ? 
                            Color.Green : Color.Gray;
                        
                        using (var pen = new Pen(lineColor, 2))
                        {
                            g.DrawLine(pen, centerX, centerY, (float)x, (float)y);
                        }

                        // Draw related node
                        var nodeColor = request.Status == selectedRequest.Status ? 
                            Color.LightGreen : Color.LightBlue;
                        DrawNode(g, (float)x, (float)y, request.Id, nodeColor);
                        
                        index++;
                    }
                }
            };

            graphPanel.Invalidate();
        }

        private void DrawNode(Graphics g, float x, float y, string id, Color color)
        {
            var nodeSize = 60;
            var rect = new RectangleF(x - nodeSize/2, y - nodeSize/2, nodeSize, nodeSize);
            
            using (var brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, rect);
            }
            
            using (var pen = new Pen(Color.DarkGray, 2))
            {
                g.DrawEllipse(pen, rect);
            }

            // Draw ID text
            using (var brush = new SolidBrush(Color.White))
            {
                var shortId = id.Substring(Math.Max(0, id.Length - 4));
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(shortId, new Font("Arial", 8), brush, new PointF(x, y), format);
            }
        }

        private void ShowEmptyState()
        {
            pnlDetails.Controls.Clear();
            
            var lblEmptyState = new Label
            {
                Text = "Select a request to view details",
                Location = new Point(15, 15),
                Size = new Size(pnlDetails.Width - 30, 30),
                Font = new Font("Segoe UI", 11F),
                ForeColor = ThemeColors.TextSecondary,
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            var iconLabel = new Label
            {
                Text = "📋",
                Location = new Point(15, 60),
                Size = new Size(pnlDetails.Width - 30, 60),
                Font = new Font("Segoe UI", 32F),
                ForeColor = ThemeColors.TextSecondary,
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            pnlDetails.Controls.AddRange(new Control[] { lblEmptyState, iconLabel });
        }

        private void ShowEmptyGraph()
        {
            if (graphPanel == null) return;
            
            graphPanel.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                
                var text = "No requests selected";
                using (var font = new Font("Segoe UI", 11F))
                using (var brush = new SolidBrush(ThemeColors.TextSecondary))
                {
                    var size = g.MeasureString(text, font);
                    g.DrawString(text, font, brush, 
                        (graphPanel.Width - size.Width) / 2,
                        (graphPanel.Height - size.Height) / 2);
                }
            };
            
            graphPanel.Invalidate();
        }
    }
}
