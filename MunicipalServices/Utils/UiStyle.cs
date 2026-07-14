using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MunicipalServices.Utils
{
    /// <summary>
    /// Shared control styling so every screen looks consistent.
    /// </summary>
    public static class UiStyle
    {
        public static readonly Font TitleFont = new Font("Segoe UI Semibold", 22F);
        public static readonly Font SubtitleFont = new Font("Segoe UI", 11F);
        public static readonly Font BodyFont = new Font("Segoe UI", 11F);
        public static readonly Font BodySemibold = new Font("Segoe UI Semibold", 11F);
        public static readonly Font NavFont = new Font("Segoe UI Semibold", 10.5F);
        public static readonly Font BrandFont = new Font("Segoe UI Semibold", 13F);
        public static readonly Font FieldLabelFont = new Font("Segoe UI Semibold", 10F);

        public static void StyleForm(Form form)
        {
            form.BackColor = ThemeColors.Background;
            form.Font = BodyFont;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.AutoScroll = true;
        }

        public static void StyleTitle(Label label)
        {
            label.Font = TitleFont;
            label.ForeColor = ThemeColors.Primary;
            label.AutoSize = true;
        }

        public static void StyleSubtitle(Label label)
        {
            label.Font = SubtitleFont;
            label.ForeColor = ThemeColors.TextSecondary;
            label.AutoSize = true;
        }

        public static void StyleFieldLabel(Label label)
        {
            label.Font = FieldLabelFont;
            label.ForeColor = ThemeColors.TextPrimary;
            label.AutoSize = true;
        }

        public static void StylePrimaryButton(Button button)
        {
            ApplyButtonBase(button, ThemeColors.Primary, Color.White);
        }

        public static void StyleAccentButton(Button button)
        {
            ApplyButtonBase(button, ThemeColors.Accent, Color.White);
        }

        public static void StyleSecondaryButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = ThemeColors.Primary;
            button.BackColor = ThemeColors.Surface;
            button.ForeColor = ThemeColors.Primary;
            button.Font = BodySemibold;
            button.Cursor = Cursors.Hand;
            button.Height = Math.Max(button.Height, 40);
            button.FlatAppearance.MouseOverBackColor = ThemeColors.PrimaryLight;
            button.FlatAppearance.MouseDownBackColor = ThemeColors.PrimaryLight;
            AttachHover(button, ThemeColors.PrimaryLight, ThemeColors.Surface);
        }

        public static void StyleMutedButton(Button button)
        {
            ApplyButtonBase(button, ThemeColors.TextSecondary, Color.White);
        }

        public static void StyleNavButton(Button button, bool isActive)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(18, 0, 12, 0);
            button.Font = NavFont;
            button.Cursor = Cursors.Hand;
            button.Height = 44;
            button.Dock = DockStyle.Top;
            button.Margin = new Padding(0);

            if (isActive)
            {
                button.BackColor = ThemeColors.SidebarActive;
                button.ForeColor = Color.White;
            }
            else
            {
                button.BackColor = ThemeColors.Sidebar;
                button.ForeColor = ThemeColors.TextOnDark;
            }

            button.MouseEnter -= NavButton_MouseEnter;
            button.MouseLeave -= NavButton_MouseLeave;
            // Preserve AppScreen in Tag; store active state via Name suffix or separate property.
            // Callers that need Tag for navigation should set Tag after this, or we keep a known pattern:
            // Form1 sets Tag = AppScreen before calling this method, so stash active on AccessibleName.
            button.AccessibleName = isActive ? "nav-active" : "nav";
            button.MouseEnter += NavButton_MouseEnter;
            button.MouseLeave += NavButton_MouseLeave;
        }

        private static void NavButton_MouseEnter(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            if (button.AccessibleName == "nav-active") return;
            button.BackColor = ThemeColors.SidebarHover;
        }

        private static void NavButton_MouseLeave(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            button.BackColor = button.AccessibleName == "nav-active"
                ? ThemeColors.SidebarActive
                : ThemeColors.Sidebar;
        }

        public static void StyleTextBox(TextBox textBox)
        {
            textBox.Font = BodyFont;
            textBox.BackColor = ThemeColors.CardBackground;
            textBox.ForeColor = ThemeColors.TextPrimary;
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        public static void StylePlaceholder(TextBox textBox, string placeholder)
        {
            StyleTextBox(textBox);
            textBox.Text = placeholder;
            textBox.ForeColor = ThemeColors.TextSecondary;

            textBox.Enter -= Placeholder_Enter;
            textBox.Leave -= Placeholder_Leave;
            textBox.Tag = placeholder;
            textBox.Enter += Placeholder_Enter;
            textBox.Leave += Placeholder_Leave;
        }

        public static void StyleRichTextBox(RichTextBox richTextBox, string placeholder = null)
        {
            richTextBox.Font = BodyFont;
            richTextBox.BackColor = ThemeColors.CardBackground;
            richTextBox.BorderStyle = BorderStyle.FixedSingle;

            if (!string.IsNullOrEmpty(placeholder))
            {
                richTextBox.Text = placeholder;
                richTextBox.ForeColor = ThemeColors.TextSecondary;
                richTextBox.Tag = placeholder;

                richTextBox.Enter -= RichPlaceholder_Enter;
                richTextBox.Leave -= RichPlaceholder_Leave;
                richTextBox.Enter += RichPlaceholder_Enter;
                richTextBox.Leave += RichPlaceholder_Leave;
            }
            else
            {
                richTextBox.ForeColor = ThemeColors.TextPrimary;
            }
        }

        public static void StyleComboBox(ComboBox comboBox)
        {
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = BodyFont;
            comboBox.BackColor = ThemeColors.CardBackground;
            comboBox.ForeColor = ThemeColors.TextPrimary;
        }

        public static void StyleListBox(ListBox listBox)
        {
            listBox.Font = BodyFont;
            listBox.BackColor = ThemeColors.CardBackground;
            listBox.ForeColor = ThemeColors.TextPrimary;
            listBox.BorderStyle = BorderStyle.FixedSingle;
            listBox.IntegralHeight = false;
        }

        public static void StylePanel(Panel panel)
        {
            panel.BackColor = ThemeColors.Surface;
            panel.Padding = new Padding(16);
        }

        public static Color StatusColor(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return ThemeColors.TextSecondary;

            switch (status.Replace(" ", "").ToLowerInvariant())
            {
                case "reported":
                case "inreview":
                    return ThemeColors.Info;
                case "assigned":
                case "inprogress":
                    return ThemeColors.Warning;
                case "resolved":
                    return ThemeColors.Success;
                case "closed":
                    return ThemeColors.Danger;
                default:
                    return ThemeColors.TextSecondary;
            }
        }

        public static Label CreateStatusChip(string status)
        {
            var color = StatusColor(status);
            return new Label
            {
                Text = status,
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9F),
                ForeColor = Color.White,
                BackColor = color,
                Padding = new Padding(8, 4, 8, 4),
                Margin = new Padding(0, 0, 8, 0)
            };
        }

        private static void ApplyButtonBase(Button button, Color backColor, Color foreColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.Font = BodySemibold;
            button.Cursor = Cursors.Hand;
            button.Height = Math.Max(button.Height, 40);
            AttachHover(button, ControlPaint.Light(backColor), backColor);
        }

        private static void AttachHover(Button button, Color hover, Color normal)
        {
            // Store colors for hover without clobbering Tag used elsewhere
            button.MouseEnter += (s, e) => button.BackColor = hover;
            button.MouseLeave += (s, e) => button.BackColor = normal;
        }

        private static void Placeholder_Enter(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;
            var placeholder = textBox.Tag as string;
            if (textBox.Text == placeholder)
            {
                textBox.Text = "";
                textBox.ForeColor = ThemeColors.TextPrimary;
            }
        }

        private static void Placeholder_Leave(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;
            var placeholder = textBox.Tag as string;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = ThemeColors.TextSecondary;
            }
        }

        private static void RichPlaceholder_Enter(object sender, EventArgs e)
        {
            var box = sender as RichTextBox;
            if (box == null) return;
            var placeholder = box.Tag as string;
            if (box.Text == placeholder)
            {
                box.Text = "";
                box.ForeColor = ThemeColors.TextPrimary;
            }
        }

        private static void RichPlaceholder_Leave(object sender, EventArgs e)
        {
            var box = sender as RichTextBox;
            if (box == null) return;
            var placeholder = box.Tag as string;
            if (string.IsNullOrWhiteSpace(box.Text))
            {
                box.Text = placeholder;
                box.ForeColor = ThemeColors.TextSecondary;
            }
        }
    }

    public static class GraphicsExtensions
    {
        public static void AddRoundedRectangle(this GraphicsPath path, Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
        }
    }
}
