using TableroDirectorio.Presenters;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class GeneralConfigControl : UserControl
{
    private readonly ConfigPresenter _presenter;
    private readonly ComboBox _themeCombo;
    private readonly Panel _themePreviewPanel;
    private readonly TextBox _titleBox;
    private readonly TextBox _subtitleBox;
    private readonly TextBox _bannerBox;
    private readonly TextBox _newPasswordBox;
    private readonly TextBox _confirmPasswordBox;

    public GeneralConfigControl(ConfigPresenter presenter)
    {
        _presenter = presenter;
        Dock = DockStyle.Fill;
        AutoScroll = true;
        BackColor = Color.Transparent;
        Padding = new Padding(25, 15, 25, 20);

        var y = 10;
        const int inputWidth = 460;

        // SECTION: APARIENCIA / TEMAS
        AddSectionHeader("🎨 Selector de Tema Minimalista", ref y);

        _themeCombo = new ComboBox
        {
            Location = new Point(0, y),
            Size = new Size(inputWidth, 34),
            Font = DarkTheme.CardBodyFont,
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextPrimary,
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat
        };
        foreach (var name in ThemeManager.ThemeNames)
        {
            _themeCombo.Items.Add(name);
        }
        _themeCombo.SelectedItem = presenter.GetTheme();
        _themeCombo.SelectedIndexChanged += OnThemeComboChanged;
        Controls.Add(_themeCombo);
        y += 40;

        // Theme preview badges
        _themePreviewPanel = new Panel
        {
            Location = new Point(0, y),
            Size = new Size(inputWidth, 30),
            BackColor = Color.Transparent
        };
        Controls.Add(_themePreviewPanel);
        UpdateThemePreview();
        y += 40;

        // SECTION: INFORMACIÓN GENERAL
        AddSectionHeader("🏢 Información Institucional", ref y);

        AddLabel("Título de la Aplicación", ref y);
        _titleBox = AddTextBox(ref y, inputWidth);
        _titleBox.Text = presenter.GetAppTitle();

        AddLabel("Subtítulo", ref y);
        _subtitleBox = AddTextBox(ref y, inputWidth);
        _subtitleBox.Text = presenter.GetAppSubtitle();

        AddLabel("Texto del Banner", ref y);
        _bannerBox = AddTextBox(ref y, inputWidth);
        _bannerBox.Text = presenter.GetBannerText();

        // SECTION: SEGURIDAD
        AddSectionHeader("🔒 Seguridad (Contraseña Maestra)", ref y);

        AddLabel("Nueva Contraseña (dejar vacío para mantener la actual)", ref y);
        _newPasswordBox = AddTextBox(ref y, inputWidth);
        _newPasswordBox.UseSystemPasswordChar = true;

        AddLabel("Confirmar Nueva Contraseña", ref y);
        _confirmPasswordBox = AddTextBox(ref y, inputWidth);
        _confirmPasswordBox.UseSystemPasswordChar = true;
    }

    private void OnThemeComboChanged(object? sender, EventArgs e)
    {
        if (_themeCombo.SelectedItem is string themeName)
        {
            ThemeManager.SetTheme(themeName);
            UpdateThemePreview();
        }
    }

    private void UpdateThemePreview()
    {
        _themePreviewPanel.Controls.Clear();
        var current = ThemeManager.Current;

        var colors = new[]
        {
            ("Fondo", current.BackgroundDark),
            ("Tarjeta", current.CardBackground),
            ("Acento", current.AccentPrimary),
            ("Secundario", current.AccentSecondary),
            ("Texto", current.TextPrimary)
        };

        int x = 0;
        foreach (var (label, color) in colors)
        {
            var badge = new Panel
            {
                Location = new Point(x, 2),
                Size = new Size(75, 24),
                BackColor = color
            };
            var lbl = new Label
            {
                Text = label,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8f),
                ForeColor = current.IsDark && (label == "Fondo" || label == "Tarjeta") ? Color.White : (label == "Texto" ? (current.IsDark ? Color.Black : Color.White) : Color.White)
            };
            badge.Controls.Add(lbl);
            _themePreviewPanel.Controls.Add(badge);
            x += 85;
        }
    }

    public bool SaveChanges()
    {
        // Save theme
        if (_themeCombo.SelectedItem is string themeName)
        {
            _presenter.SaveTheme(themeName);
            ThemeManager.SetTheme(themeName);
        }

        // Save texts
        _presenter.SaveAppTitle(_titleBox.Text.Trim());
        _presenter.SaveAppSubtitle(_subtitleBox.Text.Trim());
        _presenter.SaveBannerText(_bannerBox.Text.Trim());

        // Password change
        if (!string.IsNullOrWhiteSpace(_newPasswordBox.Text))
        {
            if (_newPasswordBox.Text != _confirmPasswordBox.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (_newPasswordBox.Text.Length < 4)
            {
                MessageBox.Show("La contraseña debe tener al menos 4 caracteres.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _presenter.ChangePassword(_newPasswordBox.Text);
        }

        return true;
    }

    private void AddSectionHeader(string text, ref int y)
    {
        var lbl = new Label
        {
            Text = text,
            Font = DarkTheme.CardTitleFont,
            ForeColor = DarkTheme.AccentSky,
            Location = new Point(0, y),
            AutoSize = true
        };
        Controls.Add(lbl);
        y += 28;
    }

    private void AddLabel(string text, ref int y)
    {
        var label = new Label
        {
            Text = text,
            Font = DarkTheme.CardBodyFont,
            ForeColor = DarkTheme.TextSecondary,
            Location = new Point(0, y),
            AutoSize = true
        };
        Controls.Add(label);
        y += 22;
    }

    private TextBox AddTextBox(ref int y, int width)
    {
        var box = new TextBox
        {
            Location = new Point(0, y),
            Size = new Size(width, 30),
            Font = DarkTheme.CardBodyFont,
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextPrimary,
            BorderStyle = BorderStyle.FixedSingle
        };
        Controls.Add(box);
        y += 38;
        return box;
    }
}
