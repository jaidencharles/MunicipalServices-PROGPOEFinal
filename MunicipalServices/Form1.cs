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
                Height = 110,
                Padding = new Padding(18, 20, 18, 12),
                BackColor = ThemeColors.Sidebar
            };

            var lblBrand = new Label
            {
                Text = "Helping Hands\nConnect",
                Font = UiStyle.BrandFont,
                ForeColor = ThemeColors.TextOnDark,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblBrandHint = new Label
            {
                Text = "Municipal services",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = ThemeColors.TextMutedOnDark,
                Dock = DockStyle.Bottom,
                Height = 20
            };

            brandPanel.Controls.Add(lblBrand);
            brandPanel.Controls.Add(lblBrandHint);

            navEvents = CreateNavButton("  Local Events", AppScreen.LocalEvents);
            navStatus = CreateNavButton("  Request Status", AppScreen.RequestStatus);
            navReports = CreateNavButton("  View Reports", AppScreen.ViewReports);
            navReport = CreateNavButton("  Report Issue", AppScreen.ReportIssue);
            navHome = CreateNavButton("  Home", AppScreen.Home);

            // Dock Top stacks upward visually last-added at top, so add in reverse.
            sidebarPanel.Controls.Add(navEvents);
            sidebarPanel.Controls.Add(navStatus);
            sidebarPanel.Controls.Add(navReports);
            sidebarPanel.Controls.Add(navReport);
            sidebarPanel.Controls.Add(navHome);
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
                Padding = new Padding(48, 36, 48, 36)
            };

            var hero = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeColors.Background
            };

            var logo = new PictureBox
            {
                Name = "pictureBoxLogo",
                Size = new Size(120, 72),
                Location = new Point(0, 8),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            LoadLogo(logo);

            var lblAppName = new Label
            {
                Text = "Helping Hands Connect",
                Font = new Font("Segoe UI Semibold", 32F),
                ForeColor = ThemeColors.Primary,
                Location = new Point(0, 92),
                AutoSize = true
            };

            var lblTagline = new Label
            {
                Text = "Report municipal issues, follow their progress, and stay connected with local events.",
                Font = UiStyle.SubtitleFont,
                ForeColor = ThemeColors.TextSecondary,
                Location = new Point(0, 150),
                MaximumSize = new Size(640, 0),
                AutoSize = true
            };

            var actions = new FlowLayoutPanel
            {
                Location = new Point(0, 210),
                Size = new Size(760, 360),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = false
            };

            actions.Controls.Add(CreateHomeAction(
                "Report an issue",
                "Tell us about roads, utilities, safety, or other municipal concerns.",
                true,
                AppScreen.ReportIssue));

            actions.Controls.Add(CreateHomeAction(
                "Check request status",
                "Look up a service request and see how it is moving forward.",
                false,
                AppScreen.RequestStatus));

            actions.Controls.Add(CreateHomeAction(
                "Local events",
                "Browse upcoming community events and announcements.",
                false,
                AppScreen.LocalEvents));

            hero.Controls.Add(logo);
            hero.Controls.Add(lblAppName);
            hero.Controls.Add(lblTagline);
            hero.Controls.Add(actions);
            homePanel.Controls.Add(hero);
        }

        private Panel CreateHomeAction(string title, string description, bool primary, AppScreen screen)
        {
            var card = new Panel
            {
                Width = 680,
                Height = 92,
                Margin = new Padding(0, 0, 0, 14),
                BackColor = ThemeColors.Surface,
                Padding = new Padding(20, 16, 20, 16)
            };

            // Thin accent bar on the left
            var accent = new Panel
            {
                Dock = DockStyle.Left,
                Width = 4,
                BackColor = primary ? ThemeColors.Accent : ThemeColors.Primary
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = UiStyle.BodySemibold,
                ForeColor = ThemeColors.TextPrimary,
                Location = new Point(22, 14),
                AutoSize = true
            };

            var lblDesc = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = ThemeColors.TextSecondary,
                Location = new Point(22, 42),
                MaximumSize = new Size(480, 0),
                AutoSize = true
            };

            var btn = new Button
            {
                Text = primary ? "Get started" : "Open",
                Size = new Size(120, 40),
                Location = new Point(520, 22)
            };

            if (primary)
                UiStyle.StyleAccentButton(btn);
            else
                UiStyle.StyleSecondaryButton(btn);

            btn.Click += (s, e) => ShowScreen(screen);
            card.Click += (s, e) => ShowScreen(screen);
            lblTitle.Click += (s, e) => ShowScreen(screen);
            lblDesc.Click += (s, e) => ShowScreen(screen);

            card.Controls.Add(btn);
            card.Controls.Add(lblDesc);
            card.Controls.Add(lblTitle);
            card.Controls.Add(accent);

            return card;
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
