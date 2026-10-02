namespace TableroDirectorio.UI.Themes;

public static class DarkTheme
{
    // Dynamic theme colors connected to ThemeManager
    public static Color BackgroundDark => ThemeManager.Current.BackgroundDark;
    public static Color BackgroundMid => ThemeManager.Current.BackgroundMid;
    public static Color BackgroundLight => ThemeManager.Current.BackgroundLight;

    public static Color AccentBlue => ThemeManager.Current.AccentPrimary;
    public static Color AccentSky => ThemeManager.Current.AccentSecondary;
    public static readonly Color AccentRed = Color.FromArgb(239, 68, 68);         // #EF4444
    public static readonly Color AccentGreen = Color.FromArgb(16, 185, 129);      // #10B981
    public static readonly Color AccentAmber = Color.FromArgb(245, 158, 11);      // #F59E0B
    public static readonly Color AccentPurple = Color.FromArgb(124, 58, 237);     // #7c3aed

    public static Color TextPrimary => ThemeManager.Current.TextPrimary;
    public static Color TextSecondary => ThemeManager.Current.TextSecondary;
    public static Color TextMuted => ThemeManager.Current.TextMuted;

    public static Color CardBackground => ThemeManager.Current.CardBackground;
    public static Color CardBorder => ThemeManager.Current.CardBorder;
    public static Color CardHover => ThemeManager.Current.CardHover;

    public static Color InputBackground => ThemeManager.Current.InputBackground;
    public static Color InputBorder => ThemeManager.Current.InputBorder;

    public static Color ButtonStart => ThemeManager.Current.AccentSecondary;
    public static Color ButtonEnd => ThemeManager.Current.AccentPrimary;

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

    public static Color GetCardBackgroundWithTint(string colorHex) =>
        ThemeManager.Current.GetCardBackgroundWithTint(colorHex);

    public static void ApplyTo(Control control)
    {
        control.BackColor = BackgroundMid;
        control.ForeColor = TextPrimary;
        control.Font = CardBodyFont;
    }

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
