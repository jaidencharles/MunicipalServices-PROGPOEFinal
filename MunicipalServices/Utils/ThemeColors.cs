using System.Drawing;

namespace MunicipalServices.Utils
{
    /// <summary>
    /// Shared palette for Helping Hands Connect.
    /// South African flag colours used in a restrained civic/municipal treatment:
    /// blue for structure, green for secondary actions, gold for primary CTAs,
    /// red for alerts, black/white for typography and surfaces.
    /// </summary>
    public static class ThemeColors
    {
        // Flag colours (approximate official values)
        public static Color FlagBlue = Color.FromArgb(0, 20, 137);      // #001489
        public static Color FlagGreen = Color.FromArgb(0, 119, 73);     // #007749
        public static Color FlagGold = Color.FromArgb(252, 181, 20);    // #FCB514
        public static Color FlagRed = Color.FromArgb(224, 60, 49);      // #E03C31
        public static Color FlagBlack = Color.FromArgb(0, 0, 0);
        public static Color FlagWhite = Color.FromArgb(255, 255, 255);

        // Brand / chrome
        public static Color Primary = FlagBlue;
        public static Color PrimaryDark = Color.FromArgb(0, 12, 90);
        public static Color PrimaryLight = Color.FromArgb(232, 236, 248); // soft blue wash
        public static Color Accent = FlagGold;
        public static Color AccentText = FlagBlack;                      // gold buttons need dark text
        public static Color Secondary = FlagGreen;

        // Surfaces
        public static Color Background = Color.FromArgb(245, 246, 248); // cool neutral
        public static Color Surface = FlagWhite;
        public static Color Sidebar = Color.FromArgb(8, 14, 48);        // deep navy (black + blue)
        public static Color SidebarHover = Color.FromArgb(18, 28, 78);
        public static Color SidebarActive = Color.FromArgb(0, 20, 137); // flag blue
        public static Color CardBackground = FlagWhite;
        public static Color RowHover = Color.FromArgb(236, 239, 247);

        // Text
        public static Color TextPrimary = Color.FromArgb(18, 18, 20);
        public static Color TextSecondary = Color.FromArgb(70, 74, 86);
        public static Color TextOnDark = Color.FromArgb(245, 246, 250);
        public static Color TextMutedOnDark = Color.FromArgb(170, 178, 200);

        // Feedback mapped to flag colours
        public static Color Success = FlagGreen;
        public static Color Warning = Color.FromArgb(196, 140, 16);     // deeper gold for text/chips
        public static Color Danger = FlagRed;
        public static Color Info = FlagBlue;

        // Chrome
        public static Color Border = Color.FromArgb(214, 218, 228);
        public static Color InputBorder = Color.FromArgb(190, 196, 210);
    }
}
