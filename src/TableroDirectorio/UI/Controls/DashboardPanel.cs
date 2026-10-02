using TableroDirectorio.Models;
using TableroDirectorio.Presenters;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class DashboardPanel : UserControl
{
    private readonly DashboardPresenter _presenter;
    private readonly Label _titleLabel;
    private readonly Label _subtitleLabel;
    private readonly BannerPillControl _bannerPill;
    private readonly CounterCardControl _counterCard;
    private readonly SearchBoxControl _searchBox;
    private readonly FlowLayoutPanel _cardsContainer;
    private readonly Panel _topBar;
    private readonly Panel _headerPanel;
    private readonly Panel _counterPanel;
    private readonly Panel _searchPanel;
    private readonly ThemeToggleControl _themeToggle;
    private readonly Label _orgLabel;
    private readonly Button _configBtn;

    public event EventHandler? ConfigRequested;

    public DashboardPanel(DashboardPresenter presenter)
    {
        _presenter = presenter;
        Dock = DockStyle.Fill;
        AutoScroll = true;
        Padding = new Padding(0, 0, 0, 40);

        // 1. TOP NAVBAR
        _topBar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 50,
            BackColor = Color.Transparent,
            Padding = new Padding(24, 8, 24, 8)
        };

        _orgLabel = new Label
        {
            Text = "Portal Institucional",
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
            AutoSize = false,
            Width = 200,
            Dock = DockStyle.Left,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _configBtn = new Button
        {
            Text = "⚙ Administración",
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
            Height = 32,
            AutoSize = true,
            Padding = new Padding(12, 0, 12, 0),
            Cursor = Cursors.Hand,
            Margin = new Padding(10, 0, 0, 0)
        };
        _configBtn.FlatAppearance.BorderSize = 1;
        _configBtn.Click += (_, _) => ConfigRequested?.Invoke(this, EventArgs.Empty);

        _themeToggle = new ThemeToggleControl
        {
            Margin = new Padding(0, 0, 8, 0)
        };
        _themeToggle.ThemeToggled += (_, _) => _presenter.SaveTheme(ThemeManager.CurrentThemeName);

        var rightPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            BackColor = Color.Transparent
        };
        rightPanel.Controls.Add(_themeToggle);
        rightPanel.Controls.Add(_configBtn);

        _topBar.Controls.Add(_orgLabel);
        _topBar.Controls.Add(rightPanel);

        // 2. HEADER PANEL (Ample height to prevent any overlap between title, subtitle, and banner)
        _headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 155,
            BackColor = Color.Transparent,
            Padding = new Padding(20, 15, 20, 5)
        };

        _titleLabel = new Label
        {
            Text = _presenter.GetTitle(),
            Font = new Font("Segoe UI", 24f, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 44
        };

        _subtitleLabel = new Label
        {
            Text = _presenter.GetSubtitle(),
            Font = new Font("Segoe UI", 12f, FontStyle.Regular),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 26
        };

        _bannerPill = new BannerPillControl
        {
            Dock = DockStyle.Top,
            BannerText = _presenter.GetBanner(),
            Height = 40
        };

        _headerPanel.Controls.Add(_bannerPill);
        _headerPanel.Controls.Add(_subtitleLabel);
        _headerPanel.Controls.Add(_titleLabel);

        // 3. COUNTER PANEL (Clean gap and centered card)
        _counterPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 115,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 6, 0, 10)
        };

        _counterCard = new CounterCardControl();
        _counterPanel.Controls.Add(_counterCard);
        _counterPanel.Resize += (_, _) =>
        {
            _counterCard.Location = new Point(
                (_counterPanel.Width - _counterCard.Width) / 2,
                (_counterPanel.Height - _counterCard.Height) / 2);
        };

        // 4. SEARCH PANEL
        _searchPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 58,
            BackColor = Color.Transparent
        };

        _searchBox = new SearchBoxControl { Width = 640 };
        _searchBox.SearchTextChanged += (_, _) =>
        {
            var items = _presenter.Search(_searchBox.Query).ToList();
            PopulateCards(items);
        };
        _searchPanel.Controls.Add(_searchBox);
        _searchPanel.Resize += (_, _) =>
        {
            _searchBox.Location = new Point(
                Math.Max(20, (_searchPanel.Width - _searchBox.Width) / 2),
                (_searchPanel.Height - _searchBox.Height) / 2);
        };

        // 5. CARDS CONTAINER
        _cardsContainer = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = true,
            BackColor = Color.Transparent,
            Padding = new Padding(28, 15, 28, 20)
        };

        // Add in reverse dock order
        Controls.Add(_cardsContainer);
        Controls.Add(_searchPanel);
        Controls.Add(_counterPanel);
        Controls.Add(_headerPanel);
        Controls.Add(_topBar);

        ThemeManager.ThemeChanged += ApplyTheme;
        ApplyTheme();
    }

    public void ApplyTheme()
    {
        BackColor = DarkTheme.BackgroundMid;

        // Header texts
        _titleLabel.ForeColor = DarkTheme.TextPrimary;
        _subtitleLabel.ForeColor = DarkTheme.TextSecondary;

        // Top bar controls (guaranteed readable in both light and dark themes)
        _orgLabel.ForeColor = DarkTheme.TextMuted;

        _configBtn.BackColor = DarkTheme.CardBackground;
        _configBtn.ForeColor = DarkTheme.TextPrimary;
        _configBtn.FlatAppearance.BorderColor = DarkTheme.CardBorder;

        Invalidate(true);
    }

    public void LoadDashboard()
    {
        var items = _presenter.LoadItems().ToList();
        PopulateCards(items);
    }

    public void RefreshData()
    {
        _titleLabel.Text = _presenter.GetTitle();
        _subtitleLabel.Text = _presenter.GetSubtitle();
        _bannerPill.BannerText = _presenter.GetBanner();
        ApplyTheme();
        LoadDashboard();
    }

    private void PopulateCards(List<DirectoryItem> items)
    {
        _cardsContainer.SuspendLayout();
        _cardsContainer.Controls.Clear();

        foreach (var item in items)
        {
            var card = new DirectoryCardControl();
            card.SetItem(item);
            card.CardClicked += (_, it) => _presenter.OpenItem(it);
            _cardsContainer.Controls.Add(card);
        }

        _counterCard.Count = items.Count;
        _cardsContainer.ResumeLayout();
    }
}
