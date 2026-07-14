using System.Drawing;

namespace MunicipalServices.Utils
{
    /// <summary>
    /// Shared palette for Helping Hands Connect.
    /// Civic / approachable: deep teal primary, warm coral accent, soft paper surfaces.
    /// </summary>
    public static class ThemeColors
    {
        // Brand
        public static Color Primary = Color.FromArgb(15, 90, 94);        // Deep teal
        public static Color PrimaryDark = Color.FromArgb(10, 68, 72);    // Darker teal
        public static Color PrimaryLight = Color.FromArgb(232, 244, 244); // Soft teal wash
        public static Color Accent = Color.FromArgb(196, 92, 62);        // Warm coral CTA

        // Surfaces
        public static Color Background = Color.FromArgb(244, 247, 246); // Soft sage-gray
        public static Color Surface = Color.FromArgb(255, 255, 255);    // White content area
        public static Color Sidebar = Color.FromArgb(12, 58, 61);       // Dark teal sidebar
        public static Color SidebarHover = Color.FromArgb(20, 78, 82);
        public static Color SidebarActive = Color.FromArgb(28, 98, 102);
        public static Color CardBackground = Color.White;

        // Text
        public static Color TextPrimary = Color.FromArgb(28, 40, 42);
        public static Color TextSecondary = Color.FromArgb(90, 106, 108);
        public static Color TextOnDark = Color.FromArgb(236, 245, 244);
        public static Color TextMutedOnDark = Color.FromArgb(168, 196, 194);

        // Feedback
        public static Color Success = Color.FromArgb(46, 140, 98);
        public static Color Warning = Color.FromArgb(196, 140, 40);
        public static Color Danger = Color.FromArgb(180, 62, 62);
        public static Color Info = Color.FromArgb(51, 122, 183);

        // Chrome
        public static Color Border = Color.FromArgb(210, 222, 220);
        public static Color InputBorder = Color.FromArgb(190, 206, 204);
    }
}
