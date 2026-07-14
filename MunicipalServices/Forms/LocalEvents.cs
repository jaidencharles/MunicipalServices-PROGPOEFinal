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
// Parent Form1 / AppScreen live in the MunicipalServices root namespace.

namespace MunicipalServices.Forms
{
    public partial class LocalEvents : Form
    {
        private readonly EventManager _eventManager;
        private readonly EventRepository _eventRepository;
        private TextBox txtSearch;
        private ComboBox cmbCategory;
        private DateTimePicker dtpDate;
        private FlowLayoutPanel pnlRecommendations;
        private Label lblRecommendations;

        public LocalEvents(EventRepository eventRepository)
        {
            InitializeComponent();
            _eventRepository = eventRepository;
            _eventManager = new EventManager();
            
            this.AutoSize = false;
            this.AutoScroll = true;
            this.MinimumSize = new Size(950, 700);
            
            InitializeFormStyle();
            _eventRepository.ClearEvents();
            AddSampleEvents();
            
            // Sync events with EventManager
            foreach (var evt in _eventRepository.GetAllEvents())
            {
                _eventManager.AddEvent(evt);
            }
            
            InitializeSearchControls();
            LoadEvents();
            SetupEventHandlers();
        }

        private void InitializeFormStyle()
        {
            UiStyle.StyleForm(this);
            this.Padding = new Padding(20);

            // Style existing title label (already created by Designer)
            if (lblTitle != null)
            {
                lblTitle.Text = "Local events";
                UiStyle.StyleTitle(lblTitle);
                lblTitle.Location = new Point(48, 25);
                lblTitle.Visible = true;
                lblTitle.BringToFront();
            }
            // Search container
            var searchContainer = new Panel
            {
                Location = new Point(48, 80),
                Size = new Size(854, 45),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            // Style search controls
            txtSearch = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 11F),
                BorderStyle = BorderStyle.FixedSingle
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(320, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 11F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            dtpDate = new DateTimePicker
            {
                Location = new Point(530, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 11F)
            };

           

            // Events list - reduced height
            lstEvents.Location = new Point(48, 140);
            lstEvents.Size = new Size(400, 300); // Smaller height
            lstEvents.Font = new Font("Segoe UI", 11F);
            lstEvents.BorderStyle = BorderStyle.FixedSingle;
            
            // Details panel - adjusted position
            var detailsPanel = new Panel
            {
                Location = new Point(480, 140),
                Size = new Size(422, 300),
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            // Recommendations section
            lblRecommendations = new Label
            {
                Location = new Point(48, 460),
                Text = "Recommended Events:",
                Font = new Font("Segoe UI Semibold", 12F),
                ForeColor = ThemeColors.Primary,
                AutoSize = true
            };

            pnlRecommendations = new FlowLayoutPanel
            {
                Location = new Point(48, 490),
                Size = new Size(832, 150),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(10)
            };

            // Add controls
            searchContainer.Controls.AddRange(new Control[] { txtSearch, cmbCategory, dtpDate, btnSearch });
            this.Controls.AddRange(new Control[] {
                lblTitle,
                searchContainer,
                lstEvents,
                detailsPanel,
                lblRecommendations,
                pnlRecommendations
            });
        }

        private TextBox CreateStyledTextBox(string placeholder, int width)
        {
            return new TextBox
            {
                Width = width,
                Height = 40,
                Font = new Font("Segoe UI", 12F),
                BorderStyle = BorderStyle.FixedSingle,
                Text = placeholder,
                Margin = new Padding(0, 0, 10, 0)
            };
        }

        private ComboBox CreateStyledComboBox(int width)
        {
            return new ComboBox
            {
                Width = width,
                Height = 40,
                Font = new Font("Segoe UI", 12F),
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 10, 0)
            };
        }

        private DateTimePicker CreateStyledDatePicker(int width)
        {
            return new DateTimePicker
            {
                Width = width,
                Height = 40,
                Font = new Font("Segoe UI", 12F),
                Format = DateTimePickerFormat.Short,
                Margin = new Padding(0, 0, 10, 0)
            };
        }

        private Button CreateStyledButton(string text, int width)
        {
            var button = new Button
            {
                Text = text,
                Width = width,
                Height = 40
            };
            UiStyle.StylePrimaryButton(button);
            return button;
        }

        private void StyleDetailsSection(Panel detailsPanel)
        {
            // Style the labels
            StyleLabel(lblEventDate, "Date:", 0);
            StyleLabel(lblLocation, "Location:", 40);
            StyleLabel(lblOrganizer, "Organizer:", 80);
            StyleLabel(lblDetails, "Details:", 120);

            // Style the details textbox
            txtEventDetails.Location = new Point(0, 160);
            txtEventDetails.Size = new Size(detailsPanel.Width - 40, detailsPanel.Height - 180);
            txtEventDetails.Multiline = true;
            txtEventDetails.ScrollBars = ScrollBars.Vertical;
            txtEventDetails.Font = new Font("Segoe UI", 11F);
            txtEventDetails.BackColor = Color.White;
            txtEventDetails.BorderStyle = BorderStyle.None;
            txtEventDetails.ReadOnly = true;

            // Add controls to details panel
            detailsPanel.Controls.AddRange(new Control[] {
                lblEventDate,
                lblLocation,
                lblOrganizer,
                lblDetails,
                txtEventDetails
            });
        }

        private void StyleLabel(Label label, string text, int yOffset)
        {
            label.Text = text;
            label.Font = new Font("Segoe UI Semibold", 11F);
            label.ForeColor = ThemeColors.Primary;
            label.Location = new Point(0, yOffset);
            label.AutoSize = true;
        }

        private void SetupEventHandlers()
        {
            lstEvents.SelectedIndexChanged += lstEvents_SelectedIndexChanged;
            btnBack.Visible = false;
            this.FormClosing += LocalEvents_FormClosing;
            
            btnSearch.Click += (s, e) => 
            {
                string searchTerm = txtSearch.Text;
                if (string.IsNullOrEmpty(searchTerm) || searchTerm == "Search events...")
                    return;
                
                _eventManager.UpdateSearchPatterns(searchTerm);
                SearchEvents(searchTerm);
            };
            
            cmbCategory.SelectedIndexChanged += (s, e) => 
            {
                string category = cmbCategory.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(category) && category != "All Categories")
                    _eventManager.UpdateSearchPatterns(category);
                FilterEvents();
            };
            
            dtpDate.ValueChanged += (s, e) => FilterEvents();
            
            txtSearch.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    SearchEvents(txtSearch.Text);
                }
            };

            btnSearch.Click += (s, e) => SearchEvents(txtSearch.Text);
        }

        private void LoadEvents()
        {
            lstEvents.Items.Clear();
            var events = _eventRepository.GetAllEvents();
            
            if (events == null || !events.Any())
            {
                lstEvents.Items.Add("No events available");
                lstEvents.Enabled = false;
                SetupInitialState();
            }
            else
            {
                foreach (var evt in events)
                {
                    lstEvents.Items.Add($"{evt.Title} - {evt.Date.ToShortDateString()}");
                }
            }
        }

        private void SetupInitialState()
        {
            lblEventDate.Text = "Date: No event selected";
            lblLocation.Text = "Location: No event selected";
            lblOrganizer.Text = "Organizer: No event selected";
            txtEventDetails.Text = "Select an event from the list to view details...";
        }

        private void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = lstEvents.SelectedIndex;
            var events = _eventRepository.GetAllEvents();
            
            if (selectedIndex >= 0 && selectedIndex < events.Count)
            {
                Event selectedEvent = events[selectedIndex];
                DisplayEventDetails(selectedEvent);
            }
            else
            {
                SetupInitialState();
            }
        }

        private void DisplayEventDetails(Event evt)
        {
            if (evt == null) return;

            lblEventDate.Text = $"Date: {evt.Date.ToLongDateString()}";
            lblLocation.Text = $"Location: {evt.Location}";
            lblOrganizer.Text = $"Organizer: {evt.Organizer}";
            txtEventDetails.Text = evt.Description;

            lblEventDate.Visible = true;
            lblLocation.Visible = true;
            lblOrganizer.Visible = true;
            lblDetails.Visible = true;
            txtEventDetails.Visible = true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var shell = FindForm() as global::MunicipalServices.Form1;
            if (shell != null)
            {
                shell.ShowScreen(global::MunicipalServices.AppScreen.Home);
            }
        }

        private void LocalEvents_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hosted in Form1 shell — no special cleanup.
        }

        private void InitializeSearchControls()
        {
            // Keep existing controls that were added by the Designer
            var existingControls = new List<Control>(this.Controls.Cast<Control>());
            this.Controls.Clear();
            
            // Re-add the title first
            if (lblTitle != null)
            {
                this.Controls.Add(lblTitle);
                lblTitle.BringToFront();
            }

            // Initialize search box
            txtSearch = new TextBox
            {
                Location = new Point(51, 100),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 11F),
                Text = "Search events...",
                ForeColor = Color.Gray
            };

            // Add event handlers for placeholder behavior
            txtSearch.GotFocus += (s, e) => {
                if (txtSearch.Text == "Search events...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.LostFocus += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "Search events...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            // Initialize category dropdown
            cmbCategory = new ComboBox
            {
                Location = new Point(270, 100),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 11F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Add categories
            cmbCategory.Items.Add("All Categories");
            foreach (var category in _eventRepository.GetAllCategories())
            {
                cmbCategory.Items.Add(category);
            }
            cmbCategory.SelectedIndex = 0;

            // Initialize date picker
            dtpDate = new DateTimePicker
            {
                Location = new Point(440, 100),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 11F)
            };

            // Initialize recommendations panel and label
            pnlRecommendations = new FlowLayoutPanel
            {
                Location = new Point(51, lstEvents.Bottom + 20),
                Size = new Size(800, 150),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Visible = false
            };

            lblRecommendations = new Label
            {
                Location = new Point(51, lstEvents.Bottom - 5),
                Text = "Recommended Events:",
                Font = new Font("Segoe UI Semibold", 12F),
                ForeColor = Color.FromArgb(64, 64, 64),
                AutoSize = true,
                Visible = false
            };

            // Add all controls in the correct order
            this.Controls.AddRange(new Control[] {
                txtSearch,
                cmbCategory,
                dtpDate,
                btnSearch,
                lstEvents,
                lblEventDate,
                lblLocation,
                lblOrganizer,
                lblDetails,
                txtEventDetails,
                lblRecommendations,
                pnlRecommendations
            });

            UiStyle.StylePrimaryButton(btnSearch);
            UiStyle.StyleListBox(lstEvents);
            btnBack.Visible = false;

            // Ensure recommendations are on top
            lblRecommendations.BringToFront();
            pnlRecommendations.BringToFront();
        }

        private void ExecuteSearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm == "Search events...")
            {
                LoadEvents();
                pnlRecommendations.Visible = false;
                lblRecommendations.Visible = false;
                return;
            }

            lstEvents.Items.Clear();
            var events = _eventRepository.GetAllEvents();
            
            // Search in title, description, and category
            var filteredEvents = events.Where(e => 
                e.Title.ToLower().Contains(searchTerm.ToLower()) ||
                e.Description.ToLower().Contains(searchTerm.ToLower()) ||
                e.Category.ToLower().Contains(searchTerm.ToLower())
            ).ToList();

            foreach (var evt in filteredEvents)
            {
                lstEvents.Items.Add($"{evt.Title} - {evt.Date.ToShortDateString()}");
            }

            // Update search history and show recommendations
            var recommendations = _eventManager.GetRecommendations(searchTerm);
            if (recommendations.Any())
            {
                DisplayRecommendations(recommendations);
                pnlRecommendations.Visible = true;
                lblRecommendations.Visible = true;
            }
        }

        private void DisplayRecommendations(List<Event> recommendations)
        {
            pnlRecommendations.Controls.Clear();
            
            foreach (var evt in recommendations)
            {
                var panel = new Panel
                {
                    Width = 280,
                    Height = 120,
                    Margin = new Padding(8),
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.None
                };

                // Enhanced shadow and border effect
                panel.Paint += (s, e) =>
                {
                    var rect = panel.ClientRectangle;
                    using (var path = new GraphicsPath())
                    {
                        path.AddRectangle(rect);
                        using (var brush = new SolidBrush(Color.FromArgb(10, 0, 0, 0)))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                    }
                    // Outer border
                    using (var pen = new Pen(Color.FromArgb(230, 230, 230), 1))
                    {
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                    // Accent line at top
                    using (var pen = new Pen(ThemeColors.Primary, 2))
                    {
                        e.Graphics.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
                    }
                };

                // Rest of the labels remain the same
                var titleLabel = new Label
                {
                    Text = evt.Title,
                    Font = new Font("Segoe UI Semibold", 11F),
                    Location = new Point(12, 15),
                    Size = new Size(260, 25),
                    ForeColor = ThemeColors.TextPrimary
                };

                var categoryLabel = new Label
                {
                    Text = $"Category: {evt.Category}",
                    Font = new Font("Segoe UI", 9.5F),
                    Location = new Point(12, 45),
                    Size = new Size(260, 20),
                    ForeColor = ThemeColors.TextSecondary
                };

                var dateLabel = new Label
                {
                    Text = evt.Date.ToShortDateString(),
                    Font = new Font("Segoe UI", 9.5F),
                    Location = new Point(12, 70),
                    Size = new Size(260, 20),
                    ForeColor = ThemeColors.TextSecondary
                };

                panel.Controls.AddRange(new Control[] { titleLabel, categoryLabel, dateLabel });
                pnlRecommendations.Controls.Add(panel);
            }
        }

        private void AddSampleEvents()
        {
            _eventRepository.AddEvent(new Event
            {
                Title = "Community Cleanup",
                Description = "Join us for a community cleanup event! Help keep our city beautiful by participating in this monthly cleanup initiative. Supplies and refreshments will be provided. Perfect for families and community groups.",
                Location = "Central Park",
                Date = DateTime.Now.AddDays(7),
                Category = "Environment",
                Organizer = "Green Initiative"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Local Farmers Market",
                Description = "Weekly farmers market featuring fresh local produce, artisanal foods, and handcrafted items. Support local farmers and artisans while enjoying live music and family activities.",
                Location = "Town Square",
                Date = DateTime.Now.AddDays(3),
                Category = "Community",
                Organizer = "Market Association"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Youth Sports Day",
                Description = "Annual sports day featuring various activities for children aged 5-15. Activities include soccer, basketball, and track events. Registration required. Lunch provided for participants.",
                Location = "Community Sports Complex",
                Date = DateTime.Now.AddDays(14),
                Category = "Sports",
                Organizer = "Youth Sports Association"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Senior Tech Workshop",
                Description = "Free technology workshop for seniors. Learn basic computer skills, smartphone usage, and internet safety. One-on-one assistance available. Bring your own device or use our computers.",
                Location = "Public Library",
                Date = DateTime.Now.AddDays(5),
                Category = "Education",
                Organizer = "Digital Literacy Foundation"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Art in the Park",
                Description = "Monthly outdoor art exhibition featuring local artists. Includes painting workshops, live demonstrations, and art sales. Family-friendly event with activities for children.",
                Location = "Riverside Park",
                Date = DateTime.Now.AddDays(10),
                Category = "Arts",
                Organizer = "Local Arts Council"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Community Health Fair",
                Description = "Free health screenings, wellness information, and fitness demonstrations. Meet local healthcare providers and learn about healthy living. Free blood pressure and diabetes screenings available.",
                Location = "Community Center",
                Date = DateTime.Now.AddDays(21),
                Category = "Health",
                Organizer = "Health Department"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Local Food Festival",
                Description = "Celebrate our city's diverse culinary scene! Sample dishes from local restaurants, food trucks, and home chefs. Cooking demonstrations and competitions throughout the day.",
                Location = "City Plaza",
                Date = DateTime.Now.AddDays(28),
                Category = "Food",
                Organizer = "Culinary Arts Association"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Job Fair",
                Description = "Connect with local employers hiring for various positions. Bring your resume! Free resume review and interview coaching available. Professional attire recommended.",
                Location = "Convention Center",
                Date = DateTime.Now.AddDays(15),
                Category = "Career",
                Organizer = "Chamber of Commerce"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Music in the Park",
                Description = "Evening concert series featuring local bands and musicians. Bring your lawn chairs and blankets. Food vendors will be available. All musical genres represented.",
                Location = "Memorial Park Amphitheater",
                Date = DateTime.Now.AddDays(4),
                Category = "Entertainment",
                Organizer = "City Arts & Culture Department"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Pet Adoption Day",
                Description = "Find your new best friend! Local shelters bringing adoptable pets. Includes free vet consultations, pet supplies, and training tips. Adoption fees may be waived for qualified adopters.",
                Location = "Animal Welfare Center",
                Date = DateTime.Now.AddDays(12),
                Category = "Community",
                Organizer = "Animal Rescue League"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Science Fair & Innovation Expo",
                Description = "Annual showcase of student projects and local tech innovations. Interactive demonstrations, robotics displays, and STEM activities for all ages. Special presentations by industry experts.",
                Location = "Science Museum",
                Date = DateTime.Now.AddDays(18),
                Category = "Education",
                Organizer = "STEM Education Alliance"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Historical Walking Tour",
                Description = "Guided tour of historic downtown landmarks. Learn about local architecture and fascinating stories from our city's past. Tour includes stops at several historic buildings with interior access.",
                Location = "Downtown Historic District",
                Date = DateTime.Now.AddDays(9),
                Category = "Culture",
                Organizer = "Historical Society"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Small Business Workshop",
                Description = "Free workshop covering business planning, marketing, and financing. Network with successful local entrepreneurs. One-on-one mentoring sessions available by appointment.",
                Location = "Business Development Center",
                Date = DateTime.Now.AddDays(25),
                Category = "Business",
                Organizer = "Economic Development Office"
            });

            _eventRepository.AddEvent(new Event
            {
                Title = "Community Garden Workshop",
                Description = "Learn organic gardening techniques, composting, and sustainable practices. Free seeds and starter plants for participants. Ongoing mentorship program available.",
                Location = "Community Garden Center",
                Date = DateTime.Now.AddDays(16),
                Category = "Environment",
                Organizer = "Urban Farming Coalition"
            });
        }

        private void FilterEvents()
        {
            lstEvents.Items.Clear();
            var events = _eventRepository.GetAllEvents();
            
            // Apply category filter
            if (cmbCategory.SelectedItem != null && cmbCategory.SelectedItem.ToString() != "All Categories")
            {
                events = events.Where(e => e.Category == cmbCategory.SelectedItem.ToString()).ToList();
            }
            
            // Apply date filter
            events = events.Where(e => e.Date.Date >= dtpDate.Value.Date).ToList();
            
            foreach (var evt in events)
            {
                lstEvents.Items.Add($"{evt.Title} - {evt.Date.ToShortDateString()}");
            }
        }

        private void SearchEvents(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm == "Search events...")
            {
                LoadEvents();
                pnlRecommendations.Visible = false;
                lblRecommendations.Visible = false;
                return;
            }

            lstEvents.Items.Clear();
            var events = _eventRepository.GetAllEvents();
            
            // Search in title, description, and category
            var filteredEvents = events.Where(e => 
                e.Title.ToLower().Contains(searchTerm.ToLower()) ||
                e.Description.ToLower().Contains(searchTerm.ToLower()) ||
                e.Category.ToLower().Contains(searchTerm.ToLower())
            ).ToList();

            foreach (var evt in filteredEvents)
            {
                lstEvents.Items.Add($"{evt.Title} - {evt.Date.ToShortDateString()}");
            }

            // Update search history and show recommendations
            var recommendations = _eventManager.GetRecommendations(searchTerm);
            if (recommendations.Any())
            {
                DisplayRecommendations(recommendations);
                pnlRecommendations.Visible = true;
                lblRecommendations.Visible = true;
            }
        }
    }
}

