namespace TableroDirectorio.UI.Themes;

public class ThemeDefinition
{
    public string Name { get; set; } = string.Empty;
    public bool IsDark { get; set; } = true;

    // Backgrounds
    public Color BackgroundDark { get; set; }
    public Color BackgroundMid { get; set; }
    public Color BackgroundLight { get; set; }

    // Cards
    public Color CardBackground { get; set; }
    public Color CardBorder { get; set; }
    public Color CardHover { get; set; }

    // Text
    public Color TextPrimary { get; set; }
    public Color TextSecondary { get; set; }
    public Color TextMuted { get; set; }

    // Input & Controls
    public Color InputBackground { get; set; }
    public Color InputBorder { get; set; }

    // Accents
    public Color AccentPrimary { get; set; }
    public Color AccentSecondary { get; set; }

    public Color GetCardBackgroundWithTint(string colorHex)
    {
        if (IsDark)
        {
            Color tint;
            try
            {
                tint = ColorTranslator.FromHtml(colorHex);
            }
            catch
            {
                tint = AccentPrimary;
            }

            return Color.FromArgb(
                (int)(tint.R * 0.08 + BackgroundLight.R * 0.92),
                (int)(tint.G * 0.08 + BackgroundLight.G * 0.92),
                (int)(tint.B * 0.08 + BackgroundLight.B * 0.92));
        }

        // In light themes, cards stay clean solid white for professional corporate look
        return CardBackground;
    }
}
