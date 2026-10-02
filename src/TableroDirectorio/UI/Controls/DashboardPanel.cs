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
    private readonly ComboBox _themeSelector;

    public event EventHandler? ConfigRequested;

    public DashboardPanel(DashboardPresenter presenter)
    {
        _presenter = presenter;
        Dock = DockStyle.Fill;
        AutoScroll = true;
        Padding = new Padding(0, 0, 0, 40);

        // 1. TOP NAVBAR (Theme Selector + Config Button)
        _topBar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 52,
            BackColor = Color.Transparent,
            Padding = new Padding(24, 8, 24, 8)
        };

        var orgLabel = new Label
        {
            Text = "Portal Institucional",
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
            ForeColor = DarkTheme.TextMuted,
            AutoSize = false,
            Width = 200,
            Dock = DockStyle.Left,
            TextAlign = ContentAlignment.MiddleLeft
        };

        var configBtn = CreateBarButton("⚙ Administración");
        configBtn.Click += (_, _) => ConfigRequested?.Invoke(this, EventArgs.Empty);

        _themeSelector = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5f),
            Width = 175,
            Height = 32,
            Cursor = Cursors.Hand
        };
        foreach (var theme in ThemeManager.ThemeNames)
        {
            _themeSelector.Items.Add(theme);
        }
        _themeSelector.SelectedItem = ThemeManager.Current.Name;
        _themeSelector.SelectedIndexChanged += OnThemeChangedByUser;

        var themeLabel = new Label
        {
            Text = "Tema:",
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = DarkTheme.TextMuted,
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleRight,
            Margin = new Padding(0, 7, 6, 0)
        };

        var rightPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            BackColor = Color.Transparent
        };
        rightPanel.Controls.Add(themeLabel);
        rightPanel.Controls.Add(_themeSelector);
        rightPanel.Controls.Add(configBtn);

        _topBar.Controls.Add(orgLabel);
        _topBar.Controls.Add(rightPanel);

        // 2. HEADER
        _headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 125,
            BackColor = Color.Transparent,
            Padding = new Padding(20, 15, 20, 10)
        };

        _titleLabel = new Label
        {
            Text = _presenter.GetTitle(),
            Font = DarkTheme.TitleFont,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 50
        };

        _subtitleLabel = new Label
        {
            Text = _presenter.GetSubtitle(),
            Font = DarkTheme.SubtitleFont,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 28
        };

        _bannerPill = new BannerPillControl
        {
            Dock = DockStyle.Top,
            BannerText = _presenter.GetBanner()
        };

        _headerPanel.Controls.Add(_bannerPill);
        _headerPanel.Controls.Add(_subtitleLabel);
        _headerPanel.Controls.Add(_titleLabel);

        // 3. COUNTER PANEL
        _counterPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 110,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 8, 0, 8)
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
            Height = 56,
            BackColor = Color.Transparent
        };

        _searchBox = new SearchBoxControl { Width = 680 };
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
            Padding = new Padding(30, 20, 30, 20)
        };

        // Add controls in reverse dock order
        Controls.Add(_cardsContainer);
        Controls.Add(_searchPanel);
        Controls.Add(_counterPanel);
        Controls.Add(_headerPanel);
        Controls.Add(_topBar);

        ThemeManager.ThemeChanged += ApplyTheme;
        ApplyTheme();
    }

    private void OnThemeChangedByUser(object? sender, EventArgs e)
    {
        if (_themeSelector.SelectedItem is string themeName)
        {
            ThemeManager.SetTheme(themeName);
            _presenter.SaveTheme(themeName);
        }
    }

    public void ApplyTheme()
    {
        BackColor = DarkTheme.BackgroundMid;
        _titleLabel.ForeColor = DarkTheme.TextPrimary;
        _subtitleLabel.ForeColor = DarkTheme.TextSecondary;
        _themeSelector.BackColor = DarkTheme.InputBackground;
        _themeSelector.ForeColor = DarkTheme.TextPrimary;

        if (_themeSelector.SelectedItem?.ToString() != ThemeManager.Current.Name)
        {
            _themeSelector.SelectedItem = ThemeManager.Current.Name;
        }

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

    private static Button CreateBarButton(string text)
    {
        var btn = new Button
        {
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Transparent,
            ForeColor = DarkTheme.TextPrimary,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
            Height = 32,
            AutoSize = true,
            Padding = new Padding(12, 0, 12, 0),
            Cursor = Cursors.Hand,
            Margin = new Padding(10, 0, 0, 0)
        };
        btn.FlatAppearance.BorderSize = 1;
        btn.FlatAppearance.BorderColor = DarkTheme.InputBorder;
        return btn;
    }
}
