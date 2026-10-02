using TableroDirectorio.Presenters;
using TableroDirectorio.UI.Controls;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Dialogs;

public class ConfigDialog : Form
{
    private readonly ConfigPresenter _presenter;
    private readonly BoardManagementControl _boardControl;
    private readonly GeneralConfigControl _generalControl;
    private readonly Button _tabBoardsBtn;
    private readonly Button _tabGeneralBtn;
    private readonly Panel _contentPanel;

    public bool HasChanges { get; private set; }

    public ConfigDialog(ConfigPresenter presenter)
    {
        _presenter = presenter;
        Text = "Panel de Administración & Gestión de Tableros";
        Size = new Size(820, 640);
        MinimumSize = new Size(750, 550);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = DarkTheme.BackgroundMid;
        ForeColor = DarkTheme.TextPrimary;

        // Top navigation tabs
        var tabContainer = new Panel
        {
            Dock = DockStyle.Top,
            Height = 46,
            BackColor = DarkTheme.BackgroundDark,
            Padding = new Padding(15, 6, 15, 0)
        };

        _tabBoardsBtn = CreateTabButton("Gestión de Tableros (Arrastrar y Soltar)", true);
        _tabBoardsBtn.Click += (_, _) => SwitchTab(true);

        _tabGeneralBtn = CreateTabButton("Información General & Seguridad", false);
        _tabGeneralBtn.Click += (_, _) => SwitchTab(false);

        tabContainer.Controls.Add(_tabBoardsBtn);
        tabContainer.Controls.Add(_tabGeneralBtn);
        _tabGeneralBtn.Left = _tabBoardsBtn.Right + 10;

        // Main content area
        _contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent
        };

        _boardControl = new BoardManagementControl(_presenter);
        _boardControl.BoardListChanged += () => HasChanges = true;

        _generalControl = new GeneralConfigControl(_presenter);

        _contentPanel.Controls.Add(_boardControl);
        _contentPanel.Controls.Add(_generalControl);

        // Bottom footer
        var footer = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 56,
            BackColor = DarkTheme.BackgroundDark,
            Padding = new Padding(15, 10, 15, 10)
        };

        var saveBtn = new Button
        {
            Text = "Guardar y Salir",
            Dock = DockStyle.Right,
            Width = 160,
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.AccentBlue,
            ForeColor = Color.White,
            Font = DarkTheme.ButtonFont,
            Cursor = Cursors.Hand
        };
        saveBtn.FlatAppearance.BorderSize = 0;
        saveBtn.Click += OnSaveAndClose;

        var cancelBtn = new Button
        {
            Text = "Cerrar",
            Dock = DockStyle.Right,
            Width = 100,
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.BackgroundLight,
            ForeColor = DarkTheme.TextMuted,
            Font = DarkTheme.ButtonFont,
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 0, 10, 0)
        };
        cancelBtn.FlatAppearance.BorderSize = 0;
        cancelBtn.Click += (_, _) => Close();

        footer.Controls.Add(cancelBtn);
        footer.Controls.Add(saveBtn);

        Controls.Add(_contentPanel);
        Controls.Add(footer);
        Controls.Add(tabContainer);

        SwitchTab(true);
    }

    private void SwitchTab(bool showBoards)
    {
        _boardControl.Visible = showBoards;
        _generalControl.Visible = !showBoards;

        if (showBoards)
        {
            _tabBoardsBtn.BackColor = DarkTheme.BackgroundMid;
            _tabBoardsBtn.ForeColor = DarkTheme.AccentSky;
            _tabGeneralBtn.BackColor = Color.Transparent;
            _tabGeneralBtn.ForeColor = DarkTheme.TextMuted;
        }
        else
        {
            _tabBoardsBtn.BackColor = Color.Transparent;
            _tabBoardsBtn.ForeColor = DarkTheme.TextMuted;
            _tabGeneralBtn.BackColor = DarkTheme.BackgroundMid;
            _tabGeneralBtn.ForeColor = DarkTheme.AccentSky;
        }
    }

    private void OnSaveAndClose(object? sender, EventArgs e)
    {
        if (_generalControl.SaveChanges())
        {
            _boardControl.SaveCurrentOrder();
            HasChanges = true;
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    private static Button CreateTabButton(string text, bool isActive)
    {
        var btn = new Button
        {
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = isActive ? DarkTheme.BackgroundMid : Color.Transparent,
            ForeColor = isActive ? DarkTheme.AccentSky : DarkTheme.TextMuted,
            Font = DarkTheme.CardTitleFont,
            Cursor = Cursors.Hand,
            Height = 40,
            AutoSize = true,
            Padding = new Padding(12, 0, 12, 0)
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }
}
