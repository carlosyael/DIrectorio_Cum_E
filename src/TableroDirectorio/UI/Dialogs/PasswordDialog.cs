using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Dialogs;

public class PasswordDialog : Form
{
    private readonly TextBox _passwordBox;
    public string EnteredPassword => _passwordBox.Text;

    public PasswordDialog()
    {
        Text = "Autenticación de Administrador";
        Size = new Size(400, 200);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = DarkTheme.BackgroundMid;
        ForeColor = DarkTheme.TextPrimary;

        var label = new Label
        {
            Text = "Ingrese la contraseña de administración:",
            Font = DarkTheme.CardTitleFont,
            ForeColor = DarkTheme.TextPrimary,
            AutoSize = true,
            Location = new Point(30, 25)
        };

        _passwordBox = new TextBox
        {
            Location = new Point(30, 60),
            Size = new Size(320, 35),
            Font = DarkTheme.SearchFont,
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextPrimary,
            UseSystemPasswordChar = true,
            BorderStyle = BorderStyle.FixedSingle
        };

        var okButton = new Button
        {
            Text = "Acceder",
            DialogResult = DialogResult.OK,
            Location = new Point(30, 105),
            Size = new Size(155, 38),
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
            Location = new Point(195, 105),
            Size = new Size(155, 38),
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.BackgroundLight,
            ForeColor = DarkTheme.TextMuted,
            Font = DarkTheme.ButtonFont,
            Cursor = Cursors.Hand
        };
        cancelButton.FlatAppearance.BorderSize = 0;

        AcceptButton = okButton;
        CancelButton = cancelButton;

        Controls.AddRange(new Control[] { label, _passwordBox, okButton, cancelButton });
    }
}
