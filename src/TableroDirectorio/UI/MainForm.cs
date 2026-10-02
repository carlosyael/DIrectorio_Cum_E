using TableroDirectorio.Data;
using TableroDirectorio.Data.Repositories;
using TableroDirectorio.Presenters;
using TableroDirectorio.Services;
using TableroDirectorio.UI.Controls;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI;

public class MainForm : Form
{
    private readonly DashboardPanel _dashboardPanel;
    private readonly ConfigPresenter _configPresenter;

    public MainForm()
    {
        // Initialize dependencies
        var connectionFactory = new SqliteConnectionFactory();
        var dbInit = new DatabaseInitializer(connectionFactory);
        dbInit.Initialize();

        var itemRepo = new DirectoryItemRepository(connectionFactory);
        var configRepo = new ConfigRepository(connectionFactory);
        var directoryService = new DirectoryService(itemRepo);
        var configService = new ConfigService(configRepo);
        var dashboardPresenter = new DashboardPresenter(directoryService, configService);
        _configPresenter = new ConfigPresenter(configService, directoryService);

        // Load configured theme
        ThemeManager.SetTheme(configService.GetTheme());

        // Form setup
        Text = $"{configService.GetAppTitle()} — Directorio de Tableros";
        Size = new Size(1280, 800);
        MinimumSize = new Size(900, 600);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = DarkTheme.BackgroundMid;
        ForeColor = DarkTheme.TextPrimary;
        Font = DarkTheme.CardBodyFont;

        ThemeManager.ThemeChanged += () =>
        {
            BackColor = DarkTheme.BackgroundMid;
            ForeColor = DarkTheme.TextPrimary;
            Invalidate(true);
        };

        var iconPath = System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Resources", "app_icon.ico");
        if (System.IO.File.Exists(iconPath))
        {
            Icon = new Icon(iconPath);
        }

        // Dashboard panel
        _dashboardPanel = new DashboardPanel(dashboardPresenter);
        _dashboardPanel.ConfigRequested += OnConfigRequested;
        Controls.Add(_dashboardPanel);

        Load += (_, _) => _dashboardPanel.LoadDashboard();
    }

    private void OnConfigRequested(object? sender, EventArgs e)
    {
        using var pwdDialog = new Dialogs.PasswordDialog();
        if (pwdDialog.ShowDialog() != DialogResult.OK)
            return;

        if (!_configPresenter.Authenticate(pwdDialog.EnteredPassword))
        {
            MessageBox.Show(
                "Contraseña incorrecta.",
                "Acceso Denegado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        using var configDialog = new Dialogs.ConfigDialog(_configPresenter);
        configDialog.ShowDialog();

        if (configDialog.HasChanges)
        {
            _dashboardPanel.RefreshData();
            Text = $"{_configPresenter.GetAppTitle()} — Directorio de Tableros";
        }
    }
}
