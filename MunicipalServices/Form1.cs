using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MunicipalServices.Forms;
using MunicipalServices.Utils;
using MunicipalServicesLibrary.Data;

namespace MunicipalServices
{
    public enum AppScreen
    {
        Home,
        ReportIssue,
        ViewReports,
        RequestStatus,
        LocalEvents
    }

    public partial class Form1 : Form
    {
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Panel homePanel;
        private Form activeForm;
        private Button activeNavButton;
        private Button navHome;
        private Button navReport;
        private Button navReports;
        private Button navStatus;
        private Button navEvents;
        private readonly IssueRepository _issueRepository = new IssueRepository();

        public Form1()
        {
            InitializeComponent();
            BuildShell();
            ShowScreen(AppScreen.Home);
        }

        private void BuildShell()
        {
            SuspendLayout();

            Text = "Helping Hands Connect";
            BackColor = ThemeColors.Background;
            MinimumSize = new Size(1024, 700);
            ClientSize = new Size(1180, 760);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            IsMdiContainer = false;

            Controls.Clear();

            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 232,
                BackColor = ThemeColors.Sidebar,
                Padding = new Padding(0, 0, 0, 16)
            };

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeColors.Background,
                Padding = new Padding(0)
            };

            BuildSidebar();
            BuildHomePanel();

            Controls.Add(contentPanel);
            Controls.Add(sidebarPanel);

            ResumeLayout(true);
        }

        private void BuildSidebar()
        {
            var brandPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 126,
                Padding = new Padding(20, 22, 20, 14),
                BackColor = ThemeColors.Sidebar
            };

            var lblBrand = new Label
            {
                Text = "Helping Hands\nConnect",
                Font = new Font("Segoe UI Semibold", 14F),
                ForeColor = ThemeColors.TextOnDark,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblBrandHint = new Label
            {
                Text = "South African municipal services",
                Font = new Font("Segoe UI", 8.25F),
                ForeColor = ThemeColors.TextMutedOnDark,
                Dock = DockStyle.Bottom,
                Height = 18
            };

            brandPanel.Controls.Add(lblBrand);
            brandPanel.Controls.Add(lblBrandHint);
            brandPanel.Paint += (s, e) =>
            {
                Color[] bands =
                {
                    ThemeColors.FlagGreen,
                    ThemeColors.FlagGold,
                    ThemeColors.FlagBlue,
                    ThemeColors.FlagRed,
                    ThemeColors.FlagBlack
                };
                int y = brandPanel.Height - 4;
                int x = 0;
                int total = brandPanel.ClientSize.Width;
                int bandWidth = Math.Max(1, total / bands.Length);
                for (int i = 0; i < bands.Length; i++)
                {
                    int w = (i == bands.Length - 1) ? (total - x) : bandWidth;
                    using (var brush = new SolidBrush(bands[i]))
                    {
                        e.Graphics.FillRectangle(brush, x, y, w, 4);
                    }
                    x += w;
                }
            };

            var navSpacer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 14,
                BackColor = ThemeColors.Sidebar
            };

            navEvents = CreateNavButton("  Local Events", AppScreen.LocalEvents);
            navStatus = CreateNavButton("  Request Status", AppScreen.RequestStatus);
            navReports = CreateNavButton("  View Reports", AppScreen.ViewReports);
            navReport = CreateNavButton("  Report Issue", AppScreen.ReportIssue);
            navHome = CreateNavButton("  Home", AppScreen.Home);

            sidebarPanel.Controls.Add(navEvents);
            sidebarPanel.Controls.Add(navStatus);
            sidebarPanel.Controls.Add(navReports);
            sidebarPanel.Controls.Add(navReport);
            sidebarPanel.Controls.Add(navHome);
            sidebarPanel.Controls.Add(navSpacer);
            sidebarPanel.Controls.Add(brandPanel);
        }

        private Button CreateNavButton(string text, AppScreen screen)
        {
            var button = new Button
            {
                Text = text,
                Tag = screen,
                Width = 232
            };
            UiStyle.StyleNavButton(button, false);
            button.Click += (s, e) => ShowScreen(screen);
            return button;
        }

        private void BuildHomePanel()
        {
            homePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeColors.Background,
                Padding = new Padding(56, 40, 56, 40)
            };

            var hero = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeColors.Background
            };

            var logo = new PictureBox
            {
                Name = "pictureBoxLogo",
                Size = new Size(128, 76),
                Location = new Point(0, 4),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = ThemeColors.Background
            };
            LoadLogo(logo);

            var ribbon = UiStyle.CreateFlagRibbon(280);
            ribbon.Location = new Point(0, 88);

            var lblAppName = new Label
            {
                Text = "Helping Hands Connect",
                Font = new Font("Segoe UI Semibold", 34F),
                ForeColor = ThemeColors.Primary,
                Location = new Point(0, 108),
                AutoSize = true,
                BackColor = ThemeColors.Background
            };

            var lblTagline = new Label
            {
                Text = "Serving communities across South Africa — report issues, track requests, and stay informed.",
                Font = new Font("Segoe UI", 12F),
                ForeColor = ThemeColors.TextSecondary,
                Location = new Point(0, 172),
                MaximumSize = new Size(640, 0),
                AutoSize = true,
                BackColor = ThemeColors.Background
            };

            var actions = new FlowLayoutPanel
            {
                Location = new Point(0, 232),
                Size = new Size(720, 360),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = false,
                BackColor = ThemeColors.Background
            };

            actions.Controls.Add(CreateHomeAction(
                "Report an issue",
                "Tell us about roads, utilities, safety, or other municipal concerns.",
                true,
                AppScreen.ReportIssue,
                true));

            actions.Controls.Add(CreateHomeAction(
                "Check request status",
                "Look up a service request and see how it is moving forward.",
                false,
                AppScreen.RequestStatus,
                true));

            actions.Controls.Add(CreateHomeAction(
                "Local events",
                "Browse upcoming community events and announcements.",
                false,
                AppScreen.LocalEvents,
                false));

            hero.Controls.Add(logo);
            hero.Controls.Add(ribbon);
            hero.Controls.Add(lblAppName);
            hero.Controls.Add(lblTagline);
            hero.Controls.Add(actions);
            homePanel.Controls.Add(hero);
        }

        private Panel CreateHomeAction(string title, string description, bool primary, AppScreen screen, bool showDivider)
        {
            // Interaction row — no card chrome, no accent bars
            var row = new Panel
            {
                Width = 700,
                Height = 88,
                Margin = new Padding(0),
                BackColor = ThemeColors.Background,
                Cursor = Cursors.Hand
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI Semibold", 13F),
                ForeColor = ThemeColors.TextPrimary,
                Location = new Point(8, 18),
                AutoSize = true,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            var lblDesc = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 10F),
                ForeColor = ThemeColors.TextSecondary,
                Location = new Point(8, 46),
                MaximumSize = new Size(500, 0),
                AutoSize = true,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            var btn = new Button
            {
                Text = primary ? "Get started" : "Open",
                Size = primary ? new Size(128, 40) : new Size(96, 40),
                Location = new Point(primary ? 560 : 592, 24),
                TabStop = true
            };

            if (primary)
                UiStyle.StyleAccentButton(btn);
            else
                UiStyle.StyleSecondaryButton(btn);

            Action go = () => ShowScreen(screen);
            btn.Click += (s, e) => go();
            row.Click += (s, e) => go();
            lblTitle.Click += (s, e) => go();
            lblDesc.Click += (s, e) => go();

            // Soft hover wash across the row
            EventHandler enter = (s, e) => row.BackColor = ThemeColors.RowHover;
            EventHandler leave = (s, e) =>
            {
                if (!row.ClientRectangle.Contains(row.PointToClient(Cursor.Position)))
                    row.BackColor = ThemeColors.Background;
            };
            row.MouseEnter += enter;
            row.MouseLeave += leave;
            lblTitle.MouseEnter += enter;
            lblTitle.MouseLeave += leave;
            lblDesc.MouseEnter += enter;
            lblDesc.MouseLeave += leave;
            btn.MouseEnter += enter;
            btn.MouseLeave += leave;

            if (showDivider)
            {
                row.Paint += (s, e) =>
                {
                    using (var pen = new Pen(ThemeColors.Border))
                    {
                        e.Graphics.DrawLine(pen, 8, row.Height - 1, row.Width - 12, row.Height - 1);
                    }
                };
            }

            row.Controls.Add(btn);
            row.Controls.Add(lblDesc);
            row.Controls.Add(lblTitle);

            return row;
        }

        private void LoadLogo(PictureBox pictureBox)
        {
            string[] candidates =
            {
                Path.Combine(Application.StartupPath, "Images", "helping hands.jpeg"),
                Path.Combine(Application.StartupPath, "Images", "hands.jpeg"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "helping hands.jpeg")
            };

            foreach (var path in candidates)
            {
                if (!File.Exists(path)) continue;
                pictureBox.Image = Image.FromFile(path);
                return;
            }
        }

        public void ShowScreen(AppScreen screen)
        {
            // Defer when the handle exists so child forms can navigate
            // without disposing themselves mid-event-handler.
            if (IsHandleCreated)
            {
                BeginInvoke(new Action(() => NavigateCore(screen)));
            }
            else
            {
                NavigateCore(screen);
            }
        }

        private void NavigateCore(AppScreen screen)
        {
            contentPanel.SuspendLayout();

            if (activeForm != null)
            {
                contentPanel.Controls.Remove(activeForm);
                activeForm.Close();
                activeForm.Dispose();
                activeForm = null;
            }

            contentPanel.Controls.Clear();

            switch (screen)
            {
                case AppScreen.Home:
                    contentPanel.Controls.Add(homePanel);
                    SetActiveNav(navHome);
                    break;

                case AppScreen.ReportIssue:
                    HostForm(new ReportIssues(), navReport);
                    break;

                case AppScreen.ViewReports:
                    HostForm(new ViewReports(_issueRepository), navReports);
                    break;

                case AppScreen.RequestStatus:
                    HostForm(new ServiceRequestStatus(), navStatus);
                    break;

                case AppScreen.LocalEvents:
                    HostForm(new LocalEvents(new EventRepository()), navEvents);
                    break;
            }

            contentPanel.ResumeLayout(true);
        }

        private void HostForm(Form childForm, Button navButton)
        {
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.AutoScroll = true;
            contentPanel.Controls.Add(childForm);
            childForm.Show();
            SetActiveNav(navButton);
        }

        private void SetActiveNav(Button navButton)
        {
            foreach (Control control in sidebarPanel.Controls)
            {
                if (control is Button button && button.Tag is AppScreen)
                {
                    UiStyle.StyleNavButton(button, button == navButton);
                }
            }
            activeNavButton = navButton;
        }

        // Kept for designer compatibility / any leftover wiring
        private void Form1_Load(object sender, EventArgs e) { }
        private void lblAppName_Click(object sender, EventArgs e) { }
        private void btnReportIssues_Click(object sender, EventArgs e) => ShowScreen(AppScreen.ReportIssue);
        private void btnLocalEvents_Click(object sender, EventArgs e) => ShowScreen(AppScreen.LocalEvents);
        private void btnRequestStatus_Click(object sender, EventArgs e) => ShowScreen(AppScreen.RequestStatus);
    }
}
