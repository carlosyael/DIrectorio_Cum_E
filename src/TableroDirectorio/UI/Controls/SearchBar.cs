using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class SearchBar : TextBox
{
    private const string DefaultPlaceholder = "🔍 Buscar tablero...";
    private bool _isPlaceholder = true;

    public SearchBar()
    {
        Font = DarkTheme.SearchFont;
        BackColor = DarkTheme.InputBackground;
        ForeColor = DarkTheme.TextMuted;
        BorderStyle = BorderStyle.None;
        Text = DefaultPlaceholder;
        Height = 40;

        GotFocus += (_, _) =>
        {
            if (_isPlaceholder)
            {
                Text = "";
                ForeColor = DarkTheme.TextPrimary;
                _isPlaceholder = false;
            }
        };

        LostFocus += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                Text = DefaultPlaceholder;
                ForeColor = DarkTheme.TextMuted;
                _isPlaceholder = true;
            }
        };
    }

    public string SearchText => _isPlaceholder ? string.Empty : Text;
}
