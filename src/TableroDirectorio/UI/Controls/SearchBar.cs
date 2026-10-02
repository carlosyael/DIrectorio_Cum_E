using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class SearchBar : TextBox
{
    private const string DefaultPlaceholder = "🔍 Buscar tablero...";
    private bool _isPlaceholder = true;
    private bool _suppressTextChanged;

    public SearchBar()
    {
        Font = DarkTheme.SearchFont;
        BackColor = DarkTheme.InputBackground;
        ForeColor = DarkTheme.TextMuted;
        BorderStyle = BorderStyle.None;
        _suppressTextChanged = true;
        Text = DefaultPlaceholder;
        _suppressTextChanged = false;
        Height = 40;

        GotFocus += (_, _) =>
        {
            if (_isPlaceholder)
            {
                _suppressTextChanged = true;
                Text = "";
                _suppressTextChanged = false;
                ForeColor = DarkTheme.TextPrimary;
                _isPlaceholder = false;
            }
        };

        LostFocus += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                _suppressTextChanged = true;
                Text = DefaultPlaceholder;
                _suppressTextChanged = false;
                ForeColor = DarkTheme.TextMuted;
                _isPlaceholder = true;
            }
        };
    }

    public string SearchText => _isPlaceholder ? string.Empty : Text;

    /// <summary>
    /// Returns true if TextChanged events should be ignored (placeholder transitions).
    /// </summary>
    public bool IsSuppressed => _suppressTextChanged;
}
