using TableroDirectorio.Models;
using TableroDirectorio.Presenters;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class DashboardPanel : UserControl
{
    private readonly DashboardPresenter _presenter;
    private readonly Label _titleLabel;
    private readonly Label _subtitleLabel;
    private readonly Label _bannerLabel;
    private readonly Label _counterNumber;
    private readonly Label _counterText;
    private readonly SearchBar _searchBar;
    private readonly FlowLayoutPanel _cardsContainer;
    private readonly Panel _headerPanel;
    private readonly Panel _counterPanel;
    private readonly Panel _counterCard;
    private readonly Panel _searchPanel;
    private readonly Panel _searchInner;

    public event EventHandler? ConfigRequested;

    public DashboardPanel(DashboardPresenter presenter)
    {
        _presenter = presenter;
        Dock = DockStyle.Fill;
        AutoScroll = true;
        Padding = new Padding(0, 0, 0, 40);

        // Header panel
        _headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 145,
            BackColor = Color.Transparent,
            Padding = new Padding(20, 35, 20, 10)
        };

        _titleLabel = new Label
        {
            Text = $"🛡️ {_presenter.GetTitle()}",
            Font = DarkTheme.TitleFont,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 55
        };

        _subtitleLabel = new Label
        {
            Text = _presenter.GetSubtitle(),
            Font = DarkTheme.SubtitleFont,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 30
        };

        _headerPanel.Controls.Add(_subtitleLabel);
        _headerPanel.Controls.Add(_titleLabel);

        // Config button (gear icon)
        var configButton = new Button
        {
            Text = "⚙️",
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Transparent,
            ForeColor = DarkTheme.TextMuted,
            Font = new Font("Segoe UI Emoji", 16f),
            Size = new Size(45, 45),
            Cursor = Cursors.Hand,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(0, 10)
        };
        configButton.FlatAppearance.BorderSize = 0;
        configButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 255, 255, 255);
        configButton.Click += (_, _) => ConfigRequested?.Invoke(this, EventArgs.Empty);
        _headerPanel.Controls.Add(configButton);

        // Banner panel
        _bannerLabel = new Label
        {
            Text = _presenter.GetBanner(),
            Font = DarkTheme.BannerFont,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Height = 38,
            Dock = DockStyle.Top,
            Padding = new Padding(20, 0, 20, 0)
        };

        // Counter panel
        _counterPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 90,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 10, 0, 10)
        };

        _counterCard = new Panel
        {
            Size = new Size(200, 68),
        };

        _counterNumber = new Label
        {
            Text = "0",
            Font = DarkTheme.CounterFont,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 44
        };

        _counterText = new Label
        {
            Text = "Tableros Disponibles",
            Font = DarkTheme.CounterLabelFont,
            TextAlign = ContentAlignment.TopCenter,
            Dock = DockStyle.Fill
        };

        _counterCard.Controls.Add(_counterText);
        _counterCard.Controls.Add(_counterNumber);
        _counterPanel.Controls.Add(_counterCard);

        _counterPanel.Resize += (_, _) =>
        {
            _counterCard.Location = new Point(
                (_counterPanel.Width - _counterCard.Width) / 2,
                (_counterPanel.Height - _counterCard.Height) / 2);
        };

        // Search panel
        _searchPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 55,
            BackColor = Color.Transparent,
            Padding = new Padding(60, 5, 60, 5)
        };

        _searchBar = new SearchBar
        {
            Dock = DockStyle.Fill,
        };
        _searchBar.TextChanged += OnSearchTextChanged;

        _searchInner = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15, 8, 15, 8)
        };
        _searchInner.Controls.Add(_searchBar);
        _searchPanel.Controls.Add(_searchInner);

        // Cards container
        _cardsContainer = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = true,
            BackColor = Color.Transparent,
            Padding = new Padding(25, 15, 25, 15)
        };

        // Add controls in reverse dock order
        Controls.Add(_cardsContainer);
        Controls.Add(_searchPanel);
        Controls.Add(_counterPanel);
        Controls.Add(_bannerLabel);
        Controls.Add(_headerPanel);

        _headerPanel.Resize += (_, _) =>
        {
            configButton.Location = new Point(_headerPanel.Width - 60, 10);
        };

        ThemeManager.ThemeChanged += ApplyTheme;
        ApplyTheme();
    }

    public void ApplyTheme()
    {
        BackColor = DarkTheme.BackgroundMid;
        _titleLabel.ForeColor = DarkTheme.TextPrimary;
        _subtitleLabel.ForeColor = DarkTheme.TextSecondary;
        _bannerLabel.ForeColor = DarkTheme.TextSecondary;
        _bannerLabel.BackColor = Color.FromArgb(20, DarkTheme.TextPrimary.R, DarkTheme.TextPrimary.G, DarkTheme.TextPrimary.B);
        _counterCard.BackColor = Color.FromArgb(20, DarkTheme.TextPrimary.R, DarkTheme.TextPrimary.G, DarkTheme.TextPrimary.B);
        _counterNumber.ForeColor = DarkTheme.AccentSky;
        _counterText.ForeColor = DarkTheme.TextMuted;
        _searchInner.BackColor = DarkTheme.InputBackground;
        _searchBar.BackColor = DarkTheme.InputBackground;
        _searchBar.ForeColor = DarkTheme.TextMuted;
        Invalidate(true);
    }

    public void LoadDashboard()
    {
        var items = _presenter.LoadItems().ToList();
        PopulateCards(items);
    }

    public void RefreshData()
    {
        _titleLabel.Text = $"🛡️ {_presenter.GetTitle()}";
        _subtitleLabel.Text = _presenter.GetSubtitle();
        _bannerLabel.Text = _presenter.GetBanner();
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

        _counterNumber.Text = items.Count.ToString();
        _cardsContainer.ResumeLayout();
    }

    private void OnSearchTextChanged(object? sender, EventArgs e)
    {
        if (_searchBar.IsSuppressed)
            return;

        var query = _searchBar.SearchText;
        var items = _presenter.Search(query).ToList();
        PopulateCards(items);
    }
}
