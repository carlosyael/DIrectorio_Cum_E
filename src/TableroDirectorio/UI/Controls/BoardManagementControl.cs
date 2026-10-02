using TableroDirectorio.Models;
using TableroDirectorio.Presenters;
using TableroDirectorio.UI.Dialogs;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class BoardManagementControl : UserControl
{
    private readonly ConfigPresenter _presenter;
    private readonly BoardOrderListBox _listBox;
    private readonly Button _saveOrderButton;
    private bool _hasUnsavedOrder;

    public event Action? BoardListChanged;

    public BoardManagementControl(ConfigPresenter presenter)
    {
        _presenter = presenter;
        Dock = DockStyle.Fill;
        BackColor = Color.Transparent;
        Padding = new Padding(24, 16, 24, 16);

        _listBox = new BoardOrderListBox
        {
            Dock = DockStyle.Fill
        };

        _saveOrderButton = CreateBtn("💾 Guardar Orden", DarkTheme.AccentSky);
        _saveOrderButton.ForeColor = Color.Black;
        _saveOrderButton.Click += OnSaveOrderClick;
        _saveOrderButton.Enabled = false;

        _listBox.OrderChanged += () =>
        {
            _hasUnsavedOrder = true;
            _saveOrderButton.Enabled = true;
        };

        // Header toolbar
        var toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 44,
            BackColor = Color.Transparent,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        var addBtn = CreateBtn("+ Nuevo Tablero", DarkTheme.AccentGreen);
        addBtn.Margin = new Padding(0, 0, 8, 0);
        addBtn.Click += OnAddClick;

        var editBtn = CreateBtn("Editar", DarkTheme.AccentBlue);
        editBtn.Margin = new Padding(0, 0, 8, 0);
        editBtn.Click += OnEditClick;

        var deleteBtn = CreateBtn("Eliminar", DarkTheme.AccentRed);
        deleteBtn.Margin = new Padding(0, 0, 12, 0);
        deleteBtn.Click += OnDeleteClick;

        var upBtn = CreateSmallBtn("▲");
        upBtn.Margin = new Padding(0, 0, 6, 0);
        upBtn.Click += (_, _) => _listBox.MoveSelectedUp();

        var downBtn = CreateSmallBtn("▼");
        downBtn.Margin = new Padding(0, 0, 12, 0);
        downBtn.Click += (_, _) => _listBox.MoveSelectedDown();

        _saveOrderButton = CreateBtn("Guardar Orden", DarkTheme.AccentSky);
        _saveOrderButton.ForeColor = Color.Black;
        _saveOrderButton.Margin = new Padding(0, 0, 0, 0);
        _saveOrderButton.Click += OnSaveOrderClick;
        _saveOrderButton.Enabled = false;

        toolbar.Controls.AddRange(new Control[] { addBtn, editBtn, deleteBtn, upBtn, downBtn, _saveOrderButton });

        // Hint label
        var hintLabel = new Label
        {
            Text = "Arrastra y suelta elementos para definir el orden en el directorio, o utiliza los botones ▲ y ▼.",
            Font = DarkTheme.CardBodyFont,
            ForeColor = DarkTheme.TextMuted,
            Dock = DockStyle.Top,
            Height = 26,
            TextAlign = ContentAlignment.MiddleLeft
        };

        // List container with subtle border
        var listContainer = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(2),
            BackColor = DarkTheme.CardBorder
        };

        listContainer.Controls.Add(_listBox);

        Controls.Add(listContainer);
        Controls.Add(hintLabel);
        Controls.Add(toolbar);

        LoadBoards();
    }

    public void LoadBoards()
    {
        var boards = _presenter.GetBoards().ToList();
        _listBox.SetItems(boards);
        _hasUnsavedOrder = false;
        _saveOrderButton.Enabled = false;
    }

    private void OnAddClick(object? sender, EventArgs e)
    {
        using var dialog = new ItemEditorDialog();
        if (dialog.ShowDialog() == DialogResult.OK && dialog.ResultItem != null)
        {
            _presenter.CreateBoard(dialog.ResultItem);
            LoadBoards();
            BoardListChanged?.Invoke();
        }
    }

    private void OnEditClick(object? sender, EventArgs e)
    {
        if (_listBox.SelectedItem is not DirectoryItem selected)
        {
            MessageBox.Show("Por favor selecciona un tablero de la lista para editar.", "Aviso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new ItemEditorDialog(selected);
        if (dialog.ShowDialog() == DialogResult.OK && dialog.ResultItem != null)
        {
            _presenter.UpdateBoard(dialog.ResultItem);
            LoadBoards();
            BoardListChanged?.Invoke();
        }
    }

    private void OnDeleteClick(object? sender, EventArgs e)
    {
        if (_listBox.SelectedItem is not DirectoryItem selected)
        {
            MessageBox.Show("Por favor selecciona un tablero para eliminar.", "Aviso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show(
            $"¿Estás seguro de eliminar el acceso '{selected.Title}'?",
            "Confirmar Eliminación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm == DialogResult.Yes)
        {
            _presenter.DeleteBoard(selected.Id);
            LoadBoards();
            BoardListChanged?.Invoke();
        }
    }

    private void OnSaveOrderClick(object? sender, EventArgs e)
    {
        SaveCurrentOrder();
    }

    public void SaveCurrentOrder()
    {
        if (!_hasUnsavedOrder) return;

        var ordered = _listBox.GetOrderedItems();
        var pairs = ordered.Select((item, idx) => (item.Id, idx + 1));
        _presenter.ReorderBoards(pairs);

        _hasUnsavedOrder = false;
        _saveOrderButton.Enabled = false;
        BoardListChanged?.Invoke();

        MessageBox.Show("Orden de tableros guardado exitosamente.", "Éxito",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static Button CreateBtn(string text, Color bg)
    {
        var btn = new Button
        {
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = bg,
            ForeColor = Color.White,
            Font = DarkTheme.ButtonFont,
            Cursor = Cursors.Hand,
            Height = 36,
            AutoSize = true,
            Padding = new Padding(10, 0, 10, 0)
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }

    private static Button CreateSmallBtn(string text)
    {
        var btn = new Button
        {
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.CardBackground,
            ForeColor = DarkTheme.TextPrimary,
            Font = DarkTheme.ButtonFont,
            Cursor = Cursors.Hand,
            Size = new Size(36, 36)
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }
}
