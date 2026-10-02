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
    private readonly Panel _searchPanel;
    private readonly Button _addButton;

    public event EventHandler? ConfigRequested;

    public DashboardPanel(DashboardPresenter presenter)
    {
        _presenter = presenter;
        Dock = DockStyle.Fill;
        AutoScroll = true;
        BackColor = DarkTheme.BackgroundMid;
        Padding = new Padding(0, 0, 0, 40);

        // Header panel
        _headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 150,
            BackColor = Color.Transparent,
            Padding = new Padding(20, 40, 20, 10)
        };

        _titleLabel = new Label
        {
            Text = $"🛡️ {_presenter.GetTitle()}",
            Font = DarkTheme.TitleFont,
            ForeColor = DarkTheme.TextPrimary,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 55
        };

        _subtitleLabel = new Label
        {
            Text = _presenter.GetSubtitle(),
            Font = DarkTheme.SubtitleFont,
            ForeColor = DarkTheme.TextSecondary,
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
            ForeColor = DarkTheme.TextSecondary,
            BackColor = Color.FromArgb(20, 255, 255, 255),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Height = 40,
            Dock = DockStyle.Top,
            Padding = new Padding(20, 0, 20, 0)
        };

        // Counter panel
        _counterPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 15, 0, 15)
        };

        var counterCard = new Panel
        {
            Size = new Size(200, 70),
            BackColor = Color.FromArgb(20, 255, 255, 255),
        };

        _counterNumber = new Label
        {
            Text = "0",
            Font = DarkTheme.CounterFont,
            ForeColor = DarkTheme.AccentSky,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 45
        };

        _counterText = new Label
        {
            Text = "Tableros Disponibles",
            Font = DarkTheme.CounterLabelFont,
            ForeColor = DarkTheme.TextMuted,
            TextAlign = ContentAlignment.TopCenter,
            Dock = DockStyle.Fill
        };

        counterCard.Controls.Add(_counterText);
        counterCard.Controls.Add(_counterNumber);
        _counterPanel.Controls.Add(counterCard);

        // Center the counter card
        _counterPanel.Resize += (_, _) =>
        {
            counterCard.Location = new Point(
                (_counterPanel.Width - counterCard.Width) / 2,
                (_counterPanel.Height - counterCard.Height) / 2);
        };

        // Search panel
        _searchPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = Color.Transparent,
            Padding = new Padding(40, 5, 100, 5)
        };

        _searchBar = new SearchBar
        {
            Dock = DockStyle.Fill,
        };
        _searchBar.TextChanged += OnSearchTextChanged;

        _addButton = new Button
        {
            Text = "➕ Nuevo",
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.AccentGreen,
            ForeColor = Color.White,
            Font = DarkTheme.ButtonFont,
            Dock = DockStyle.Right,
            Width = 100,
            Cursor = Cursors.Hand,
            Margin = new Padding(10, 0, 0, 0)
        };
        _addButton.FlatAppearance.BorderSize = 0;
        _addButton.Click += OnAddButtonClick;

        // Wrap search in a bordered panel for visual effect
        var searchInner = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = DarkTheme.InputBackground,
            Padding = new Padding(15, 8, 15, 8)
        };
        searchInner.Controls.Add(_searchBar);

        _searchPanel.Controls.Add(searchInner);
        _searchPanel.Controls.Add(_addButton);

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

        // Position config button after layout
        _headerPanel.Resize += (_, _) =>
        {
            configButton.Location = new Point(_headerPanel.Width - 60, 10);
        };
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
            card.EditRequested += OnEditRequested;
            card.DeleteRequested += OnDeleteRequested;
            _cardsContainer.Controls.Add(card);
        }

        _counterNumber.Text = items.Count.ToString();
        _cardsContainer.ResumeLayout();
    }

    private void OnSearchTextChanged(object? sender, EventArgs e)
    {
        var query = _searchBar.SearchText;
        var items = _presenter.Search(query).ToList();
        PopulateCards(items);
    }

    private void OnAddButtonClick(object? sender, EventArgs e)
    {
        using var dialog = new Dialogs.ItemEditorDialog();
        if (dialog.ShowDialog() == DialogResult.OK && dialog.ResultItem != null)
        {
            _presenter.AddItem(dialog.ResultItem);
            LoadDashboard();
        }
    }

    private void OnEditRequested(object? sender, DirectoryItem item)
    {
        using var dialog = new Dialogs.ItemEditorDialog(item);
        if (dialog.ShowDialog() == DialogResult.OK && dialog.ResultItem != null)
        {
            _presenter.EditItem(dialog.ResultItem);
            LoadDashboard();
        }
    }

    private void OnDeleteRequested(object? sender, DirectoryItem item)
    {
        var result = MessageBox.Show(
            $"¿Estás seguro de eliminar '{item.Title}'?",
            "Confirmar Eliminación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            _presenter.RemoveItem(item.Id);
            LoadDashboard();
        }
    }
}
