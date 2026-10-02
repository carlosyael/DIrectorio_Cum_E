using TableroDirectorio.Presenters;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class GeneralConfigControl : UserControl
{
    private readonly ConfigPresenter _presenter;
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
        Padding = new Padding(25, 20, 25, 20);

        var y = 10;
        const int inputWidth = 480;

        // SECTION: INFORMACIÓN INSTITUCIONAL
        AddSectionHeader("Información Institucional", ref y);

        AddLabel("Título del Directorio", ref y);
        _titleBox = AddTextBox(ref y, inputWidth);
        _titleBox.Text = presenter.GetAppTitle();

        AddLabel("Subtítulo", ref y);
        _subtitleBox = AddTextBox(ref y, inputWidth);
        _subtitleBox.Text = presenter.GetAppSubtitle();

        AddLabel("Texto de Banner", ref y);
        _bannerBox = AddTextBox(ref y, inputWidth);
        _bannerBox.Text = presenter.GetBannerText();

        y += 10;

        // SECTION: SEGURIDAD
        AddSectionHeader("Seguridad — Contraseña Maestra", ref y);

        AddLabel("Nueva Contraseña (dejar en blanco para mantener la actual)", ref y);
        _newPasswordBox = AddTextBox(ref y, inputWidth);
        _newPasswordBox.UseSystemPasswordChar = true;

        AddLabel("Confirmar Nueva Contraseña", ref y);
        _confirmPasswordBox = AddTextBox(ref y, inputWidth);
        _confirmPasswordBox.UseSystemPasswordChar = true;
    }

    public bool SaveChanges()
    {
        _presenter.SaveAppTitle(_titleBox.Text.Trim());
        _presenter.SaveAppSubtitle(_subtitleBox.Text.Trim());
        _presenter.SaveBannerText(_bannerBox.Text.Trim());

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
        y += 30;
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
        y += 24;
    }

    private TextBox AddTextBox(ref int y, int width)
    {
        var box = new TextBox
        {
            Location = new Point(0, y),
            Size = new Size(width, 32),
            Font = DarkTheme.CardBodyFont,
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextPrimary,
            BorderStyle = BorderStyle.FixedSingle
        };
        Controls.Add(box);
        y += 40;
        return box;
    }
}
