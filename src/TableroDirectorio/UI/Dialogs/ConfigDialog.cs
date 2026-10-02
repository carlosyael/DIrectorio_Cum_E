using TableroDirectorio.Presenters;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Dialogs;

public class ConfigDialog : Form
{
    private readonly ConfigPresenter _presenter;
    private readonly TextBox _titleBox;
    private readonly TextBox _subtitleBox;
    private readonly TextBox _bannerBox;
    private readonly TextBox _newPasswordBox;
    private readonly TextBox _confirmPasswordBox;

    public ConfigDialog(ConfigPresenter presenter)
    {
        _presenter = presenter;
        Text = "⚙️ Configuración";
        Size = new Size(520, 460);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = DarkTheme.BackgroundMid;
        ForeColor = DarkTheme.TextPrimary;

        var y = 20;

        // Title
        AddLabel("Título de la Aplicación", ref y);
        _titleBox = AddTextBox(ref y);
        _titleBox.Text = presenter.GetAppTitle();

        // Subtitle
        AddLabel("Subtítulo", ref y);
        _subtitleBox = AddTextBox(ref y);
        _subtitleBox.Text = presenter.GetAppSubtitle();

        // Banner
        AddLabel("Texto del Banner", ref y);
        _bannerBox = AddTextBox(ref y);
        _bannerBox.Text = presenter.GetBannerText();

        // Separator
        var separator = new Label
        {
            Text = "── Cambiar Contraseña Maestra ──",
            Font = DarkTheme.CardTitleFont,
            ForeColor = DarkTheme.AccentSky,
            Location = new Point(30, y + 10),
            AutoSize = true
        };
        Controls.Add(separator);
        y += 40;

        AddLabel("Nueva Contraseña", ref y);
        _newPasswordBox = AddTextBox(ref y);
        _newPasswordBox.UseSystemPasswordChar = true;

        AddLabel("Confirmar Contraseña", ref y);
        _confirmPasswordBox = AddTextBox(ref y);
        _confirmPasswordBox.UseSystemPasswordChar = true;

        // Save button
        var saveBtn = new Button
        {
            Text = "Guardar Configuración",
            Location = new Point(30, y + 10),
            Size = new Size(210, 40),
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.AccentBlue,
            ForeColor = Color.White,
            Font = DarkTheme.ButtonFont,
            Cursor = Cursors.Hand
        };
        saveBtn.FlatAppearance.BorderSize = 0;
        saveBtn.Click += OnSaveClick;

        var cancelBtn = new Button
        {
            Text = "Cancelar",
            DialogResult = DialogResult.Cancel,
            Location = new Point(260, y + 10),
            Size = new Size(210, 40),
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.BackgroundLight,
            ForeColor = DarkTheme.TextMuted,
            Font = DarkTheme.ButtonFont,
            Cursor = Cursors.Hand
        };
        cancelBtn.FlatAppearance.BorderSize = 0;

        CancelButton = cancelBtn;
        Controls.AddRange(new Control[] { saveBtn, cancelBtn });
    }

    private void AddLabel(string text, ref int y)
    {
        var label = new Label
        {
            Text = text,
            Font = DarkTheme.CardBodyFont,
            ForeColor = DarkTheme.TextSecondary,
            Location = new Point(30, y),
            AutoSize = true
        };
        Controls.Add(label);
        y += 22;
    }

    private TextBox AddTextBox(ref int y)
    {
        var box = new TextBox
        {
            Location = new Point(30, y),
            Size = new Size(440, 30),
            Font = DarkTheme.CardBodyFont,
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextPrimary,
            BorderStyle = BorderStyle.FixedSingle
        };
        Controls.Add(box);
        y += 35;
        return box;
    }

    private void OnSaveClick(object? sender, EventArgs e)
    {
        // Save title/subtitle/banner
        _presenter.SaveAppTitle(_titleBox.Text.Trim());
        _presenter.SaveAppSubtitle(_subtitleBox.Text.Trim());
        _presenter.SaveBannerText(_bannerBox.Text.Trim());

        // Handle password change
        if (!string.IsNullOrWhiteSpace(_newPasswordBox.Text))
        {
            if (_newPasswordBox.Text != _confirmPasswordBox.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_newPasswordBox.Text.Length < 4)
            {
                MessageBox.Show("La contraseña debe tener al menos 4 caracteres.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _presenter.ChangePassword(_newPasswordBox.Text);
        }

        MessageBox.Show("Configuración guardada exitosamente.", "Éxito",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
        Close();
    }
}
