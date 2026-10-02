namespace TableroDirectorio.UI.Themes;

public static class ThemeManager
{
    private static readonly ThemeDefinition DarkThemeDef;
    private static readonly ThemeDefinition LightThemeDef;
    private static ThemeDefinition _current;

    public static event Action? ThemeChanged;

    static ThemeManager()
    {
        // 1. Modo Oscuro (Elegante azul medianoche corporativo)
        DarkThemeDef = new ThemeDefinition
        {
            Name = "Oscuro",
            IsDark = true,
            BackgroundDark = Color.FromArgb(2, 6, 23),       // #020617
            BackgroundMid = Color.FromArgb(15, 23, 42),      // #0f172a
            BackgroundLight = Color.FromArgb(30, 41, 59),    // #1e293b
            CardBackground = Color.FromArgb(28, 38, 54),
            CardBorder = Color.FromArgb(50, 65, 88),
            CardHover = Color.FromArgb(38, 50, 70),
            TextPrimary = Color.White,
            TextSecondary = Color.FromArgb(219, 230, 254),   // #dbeafe
            TextMuted = Color.FromArgb(148, 163, 184),       // #94a3b8
            InputBackground = Color.FromArgb(20, 30, 48),
            InputBorder = Color.FromArgb(50, 70, 105),
            AccentPrimary = Color.FromArgb(37, 99, 246),     // #2563eb
            AccentSecondary = Color.FromArgb(56, 189, 248)   // #38bdf8
        };

        // 2. Modo Claro (Clean, nítido y profesional)
        LightThemeDef = new ThemeDefinition
        {
            Name = "Claro",
            IsDark = false,
            BackgroundDark = Color.FromArgb(241, 245, 249),   // #f1f5f9
            BackgroundMid = Color.FromArgb(248, 250, 252),   // #f8fafc
            BackgroundLight = Color.FromArgb(255, 255, 255),  // #ffffff
            CardBackground = Color.FromArgb(255, 255, 255),  // Blanco sólido puro
            CardBorder = Color.FromArgb(226, 232, 240),      // #e2e8f0
            CardHover = Color.FromArgb(241, 245, 249),
            TextPrimary = Color.FromArgb(15, 23, 42),        // #0f172a
            TextSecondary = Color.FromArgb(51, 65, 85),      // #334155
            TextMuted = Color.FromArgb(100, 116, 139),       // #64748b
            InputBackground = Color.FromArgb(255, 255, 255),
            InputBorder = Color.FromArgb(203, 213, 225),
            AccentPrimary = Color.FromArgb(37, 99, 246),     // #2563eb
            AccentSecondary = Color.FromArgb(2, 132, 199)    // #0284c7
        };

        _current = DarkThemeDef;
    }

    public static ThemeDefinition Current => _current;

    public static bool IsDark => _current.IsDark;

    public static string CurrentThemeName => _current.IsDark ? "Oscuro" : "Claro";

    public static void ToggleTheme()
    {
        _current = _current.IsDark ? LightThemeDef : DarkThemeDef;
        ThemeChanged?.Invoke();
    }

    public static void SetTheme(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return;

        var lower = name.ToLowerInvariant();
        if (lower.Contains("claro") || lower.Contains("light"))
        {
            _current = LightThemeDef;
        }
        else
        {
            _current = DarkThemeDef;
        }

        ThemeChanged?.Invoke();
    }
}
