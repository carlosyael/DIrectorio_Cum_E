namespace TableroDirectorio.UI.Themes;

public static class ThemeManager
{
    private static readonly Dictionary<string, ThemeDefinition> _themes = new(StringComparer.OrdinalIgnoreCase);
    private static ThemeDefinition _current;

    public static event Action? ThemeChanged;

    static ThemeManager()
    {
        // 1. Azul Medianoche (Default inspirado en HTML template)
        var midnightBlue = new ThemeDefinition
        {
            Name = "Azul Medianoche",
            IsDark = true,
            BackgroundDark = Color.FromArgb(2, 6, 23),       // #020617
            BackgroundMid = Color.FromArgb(15, 23, 42),      // #0f172a
            BackgroundLight = Color.FromArgb(30, 41, 59),    // #1e293b
            CardBackground = Color.FromArgb(40, 50, 70),
            CardBorder = Color.FromArgb(60, 70, 90),
            CardHover = Color.FromArgb(50, 60, 80),
            TextPrimary = Color.White,
            TextSecondary = Color.FromArgb(219, 230, 254),   // #dbeafe
            TextMuted = Color.FromArgb(203, 213, 225),       // #cbd5e1
            InputBackground = Color.FromArgb(20, 30, 50),
            InputBorder = Color.FromArgb(60, 80, 120),
            AccentPrimary = Color.FromArgb(37, 99, 246),     // #2563eb
            AccentSecondary = Color.FromArgb(56, 189, 248)   // #38bdf8
        };

        // 2. Oscuro Minimalista (Carbón sobrio)
        var minimalistDark = new ThemeDefinition
        {
            Name = "Oscuro Minimalista",
            IsDark = true,
            BackgroundDark = Color.FromArgb(17, 24, 39),      // #111827
            BackgroundMid = Color.FromArgb(24, 33, 47),
            BackgroundLight = Color.FromArgb(31, 41, 55),     // #1f2937
            CardBackground = Color.FromArgb(31, 41, 55),
            CardBorder = Color.FromArgb(55, 65, 81),         // #374151
            CardHover = Color.FromArgb(45, 55, 72),
            TextPrimary = Color.FromArgb(249, 250, 251),     // #f9fafb
            TextSecondary = Color.FromArgb(209, 213, 219),   // #d1d5db
            TextMuted = Color.FromArgb(156, 163, 175),       // #9ca3af
            InputBackground = Color.FromArgb(17, 24, 39),
            InputBorder = Color.FromArgb(75, 85, 99),
            AccentPrimary = Color.FromArgb(59, 130, 246),    // #3b82f6
            AccentSecondary = Color.FromArgb(96, 165, 250)
        };

        // 3. Claro Minimalista (Clean & Crisp)
        var minimalistLight = new ThemeDefinition
        {
            Name = "Claro Minimalista",
            IsDark = false,
            BackgroundDark = Color.FromArgb(241, 245, 249),   // #f1f5f9
            BackgroundMid = Color.FromArgb(248, 250, 252),   // #f8fafc
            BackgroundLight = Color.FromArgb(255, 255, 255),  // #ffffff
            CardBackground = Color.FromArgb(255, 255, 255),
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

        // 4. Pizarra Nórdica (Slate & Emerald)
        var slateNordic = new ThemeDefinition
        {
            Name = "Pizarra Nórdica",
            IsDark = true,
            BackgroundDark = Color.FromArgb(15, 20, 25),
            BackgroundMid = Color.FromArgb(23, 28, 36),
            BackgroundLight = Color.FromArgb(38, 45, 56),
            CardBackground = Color.FromArgb(30, 37, 48),
            CardBorder = Color.FromArgb(54, 65, 83),
            CardHover = Color.FromArgb(42, 51, 66),
            TextPrimary = Color.FromArgb(243, 244, 246),
            TextSecondary = Color.FromArgb(209, 213, 219),
            TextMuted = Color.FromArgb(148, 163, 184),
            InputBackground = Color.FromArgb(18, 24, 32),
            InputBorder = Color.FromArgb(71, 85, 105),
            AccentPrimary = Color.FromArgb(16, 185, 129),    // #10b981
            AccentSecondary = Color.FromArgb(52, 211, 153)
        };

        _themes[midnightBlue.Name] = midnightBlue;
        _themes[minimalistDark.Name] = minimalistDark;
        _themes[minimalistLight.Name] = minimalistLight;
        _themes[slateNordic.Name] = slateNordic;

        _current = midnightBlue;
    }

    public static IReadOnlyList<string> ThemeNames => _themes.Keys.ToList();

    public static ThemeDefinition Current => _current;

    public static void SetTheme(string name)
    {
        if (_themes.TryGetValue(name, out var theme))
        {
            _current = theme;
            ThemeChanged?.Invoke();
        }
    }
}
