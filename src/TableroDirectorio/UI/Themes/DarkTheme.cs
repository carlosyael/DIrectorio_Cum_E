namespace TableroDirectorio.UI.Themes;

public static class DarkTheme
{
    // Background colors
    public static readonly Color BackgroundDark = Color.FromArgb(2, 6, 23);       // #020617
    public static readonly Color BackgroundMid = Color.FromArgb(15, 23, 42);      // #0f172a
    public static readonly Color BackgroundLight = Color.FromArgb(30, 41, 59);    // #1e293b

    // Accent colors
    public static readonly Color AccentBlue = Color.FromArgb(37, 99, 246);        // #2563eb
    public static readonly Color AccentSky = Color.FromArgb(56, 189, 248);        // #38bdf8
    public static readonly Color AccentRed = Color.FromArgb(239, 68, 68);         // #EF4444
    public static readonly Color AccentGreen = Color.FromArgb(16, 185, 129);      // #10B981
    public static readonly Color AccentAmber = Color.FromArgb(245, 158, 11);      // #F59E0B
    public static readonly Color AccentPurple = Color.FromArgb(124, 58, 237);     // #7c3aed

    // Text colors
    public static readonly Color TextPrimary = Color.White;
    public static readonly Color TextSecondary = Color.FromArgb(219, 230, 254);   // #dbeafe
    public static readonly Color TextMuted = Color.FromArgb(203, 213, 225);       // #cbd5e1

    // Card colors
    public static readonly Color CardBackground = Color.FromArgb(40, 50, 70);
    public static readonly Color CardBorder = Color.FromArgb(60, 70, 90);
    public static readonly Color CardHover = Color.FromArgb(50, 60, 80);

    // Input
    public static readonly Color InputBackground = Color.FromArgb(20, 30, 50);
    public static readonly Color InputBorder = Color.FromArgb(60, 80, 120);

    // Button gradient
    public static readonly Color ButtonStart = Color.FromArgb(56, 189, 248);      // #38bdf8
    public static readonly Color ButtonEnd = Color.FromArgb(37, 99, 246);         // #2563eb

    // Fonts
    public static readonly Font TitleFont = new("Segoe UI", 28f, FontStyle.Bold);
    public static readonly Font SubtitleFont = new("Segoe UI", 14f, FontStyle.Regular);
    public static readonly Font CardTitleFont = new("Segoe UI", 12f, FontStyle.Bold);
    public static readonly Font CardBodyFont = new("Segoe UI", 10f, FontStyle.Regular);
    public static readonly Font ButtonFont = new("Segoe UI", 10f, FontStyle.Bold);
    public static readonly Font SearchFont = new("Segoe UI", 12f, FontStyle.Regular);
    public static readonly Font BannerFont = new("Segoe UI", 11f, FontStyle.Regular);
    public static readonly Font CounterFont = new("Segoe UI", 36f, FontStyle.Bold);
    public static readonly Font CounterLabelFont = new("Segoe UI", 10f, FontStyle.Regular);

    public static Color GetCardColor(string colorHex)
    {
        try
        {
            return ColorTranslator.FromHtml(colorHex);
        }
        catch
        {
            return AccentBlue;
        }
    }

    public static Color GetCardBackgroundWithTint(string colorHex)
    {
        var tint = GetCardColor(colorHex);
        return Color.FromArgb(
            (int)(tint.R * 0.15 + BackgroundLight.R * 0.85),
            (int)(tint.G * 0.15 + BackgroundLight.G * 0.85),
            (int)(tint.B * 0.15 + BackgroundLight.B * 0.85));
    }

    public static void ApplyTo(Control control)
    {
        control.BackColor = BackgroundMid;
        control.ForeColor = TextPrimary;
        control.Font = CardBodyFont;
    }

    /// <summary>Gets an emoji icon based on icon name from the directory item.</summary>
    public static string GetIcon(string iconName)
    {
        return iconName?.ToLowerInvariant() switch
        {
            "chart-line" or "chart" => "📊",
            "warning" or "triangle-exclamation" => "⚠️",
            "calendar" or "calendar-days" => "📅",
            "search" or "magnifying-glass-chart" => "🔍",
            "file-excel" or "excel" => "📗",
            "globe" or "webpage" => "🌐",
            "folder" => "📁",
            "shield" => "🛡️",
            "database" => "🗄️",
            "users" => "👥",
            "gear" or "settings" => "⚙️",
            _ => "📋"
        };
    }
}
