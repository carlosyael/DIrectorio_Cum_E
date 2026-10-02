using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Dialogs;

public class PasswordDialog : Form
{
    private readonly TextBox _passwordBox;
    public string EnteredPassword => _passwordBox.Text;

    public PasswordDialog()
    {
        Text = "Acceso Administrativo";
        Size = new Size(490, 240);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = DarkTheme.BackgroundMid;
        ForeColor = DarkTheme.TextPrimary;

        var headerLabel = new Label
        {
            Text = "Autenticación de Administrador",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            ForeColor = DarkTheme.TextPrimary,
            Location = new Point(32, 22),
            Size = new Size(420, 26),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var subtitleLabel = new Label
        {
            Text = "Ingrese la contraseña maestra para gestionar los tableros:",
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
            ForeColor = DarkTheme.TextSecondary,
            Location = new Point(32, 50),
            Size = new Size(420, 22),
            TextAlign = ContentAlignment.MiddleLeft
        };

        _passwordBox = new TextBox
        {
            Location = new Point(32, 82),
            Size = new Size(410, 32),
            Font = new Font("Segoe UI", 11.5f),
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextPrimary,
            UseSystemPasswordChar = true,
            BorderStyle = BorderStyle.FixedSingle
        };

        var okButton = new Button
        {
            Text = "Acceder",
            DialogResult = DialogResult.OK,
            Location = new Point(170, 134),
            Size = new Size(130, 36),
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.AccentBlue,
            ForeColor = Color.White,
            Font = DarkTheme.ButtonFont,
            Cursor = Cursors.Hand
        };
        okButton.FlatAppearance.BorderSize = 0;

        var cancelButton = new Button
        {
            Text = "Cancelar",
            DialogResult = DialogResult.Cancel,
            Location = new Point(312, 134),
            Size = new Size(130, 36),
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.CardBackground,
            ForeColor = DarkTheme.TextPrimary,
            Font = DarkTheme.ButtonFont,
            Cursor = Cursors.Hand
        };
        cancelButton.FlatAppearance.BorderSize = 1;
        cancelButton.FlatAppearance.BorderColor = DarkTheme.CardBorder;

        AcceptButton = okButton;
        CancelButton = cancelButton;

        Controls.AddRange(new Control[] { headerLabel, subtitleLabel, _passwordBox, okButton, cancelButton });
    }
}
