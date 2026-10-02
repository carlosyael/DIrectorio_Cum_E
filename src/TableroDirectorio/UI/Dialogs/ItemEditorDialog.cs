using TableroDirectorio.Models;
using TableroDirectorio.UI.Controls;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Dialogs;

public class ItemEditorDialog : Form
{
    private readonly TextBox _titleBox;
    private readonly TextBox _descriptionBox;
    private readonly ComboBox _resourceTypeBox;
    private readonly TextBox _pathBox;
    private readonly ComboBox _iconBox;
    private readonly TextBox _categoryBox;
    private readonly ColorPickerControl _colorPicker;
    private readonly NumericUpDown _sortOrderBox;

    public DirectoryItem? ResultItem { get; private set; }
    private readonly DirectoryItem? _existingItem;

    public ItemEditorDialog(DirectoryItem? item = null)
    {
        _existingItem = item;
        Text = item == null ? "Nuevo Acceso Directo" : "Editar Acceso Directo";
        Size = new Size(540, 640);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = DarkTheme.BackgroundMid;
        ForeColor = DarkTheme.TextPrimary;
        AutoScroll = true;

        var y = 20;
        var inputWidth = 440;

        // Title
        AddLabel("Título *", ref y);
        _titleBox = AddTextBox(ref y);

        // Description
        AddLabel("Descripción", ref y);
        _descriptionBox = AddTextBox(ref y);
        _descriptionBox.Height = 50;
        _descriptionBox.Multiline = true;
        y += 20;

        // Resource Type
        AddLabel("Tipo de Recurso", ref y);
        _resourceTypeBox = new ComboBox
        {
            Location = new Point(30, y),
            Size = new Size(inputWidth, 30),
            Font = DarkTheme.CardBodyFont,
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextPrimary,
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat
        };
        _resourceTypeBox.Items.AddRange(new[] { "WebPage", "PowerBI", "Excel", "Folder", "Other" });
        _resourceTypeBox.SelectedIndex = 0;
        Controls.Add(_resourceTypeBox);
        y += 38;

        // Path
        AddLabel("Ruta / URL *", ref y);
        var pathPanel = new Panel
        {
            Location = new Point(30, y),
            Size = new Size(inputWidth, 30),
            BackColor = Color.Transparent
        };
        _pathBox = new TextBox
        {
            Dock = DockStyle.Fill,
            Font = DarkTheme.CardBodyFont,
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextPrimary,
            BorderStyle = BorderStyle.FixedSingle
        };
        var browseBtn = new Button
        {
            Text = "...",
            Dock = DockStyle.Right,
            Width = 40,
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.BackgroundLight,
            ForeColor = DarkTheme.TextPrimary,
            Cursor = Cursors.Hand
        };
        browseBtn.FlatAppearance.BorderSize = 0;
        browseBtn.Click += OnBrowseClick;
        pathPanel.Controls.Add(_pathBox);
        pathPanel.Controls.Add(browseBtn);
        Controls.Add(pathPanel);
        y += 38;

        // Icon
        AddLabel("Icono", ref y);
        _iconBox = new ComboBox
        {
            Location = new Point(30, y),
            Size = new Size(inputWidth, 30),
            Font = DarkTheme.CardBodyFont,
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextPrimary,
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat
        };
        _iconBox.Items.AddRange(new[] { "chart-line", "warning", "calendar", "search", "file-excel", "globe", "folder", "shield", "database", "users", "gear" });
        _iconBox.SelectedIndex = 0;
        Controls.Add(_iconBox);
        y += 38;

        // Category
        AddLabel("Categoría", ref y);
        _categoryBox = AddTextBox(ref y);

        // Color
        AddLabel("Color del Tablero", ref y);
        _colorPicker = new ColorPickerControl
        {
            Location = new Point(30, y),
            Width = inputWidth
        };
        Controls.Add(_colorPicker);
        y += 84;

        // Sort Order
        AddLabel("Orden", ref y);
        _sortOrderBox = new NumericUpDown
        {
            Location = new Point(30, y),
            Size = new Size(100, 30),
            Font = DarkTheme.CardBodyFont,
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextPrimary,
            Minimum = 0,
            Maximum = 999
        };
        Controls.Add(_sortOrderBox);
        y += 38;

        // Buttons
        var saveBtn = new Button
        {
            Text = item == null ? "Crear" : "Guardar",
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

        // If editing, populate fields
        if (item != null)
        {
            _titleBox.Text = item.Title;
            _descriptionBox.Text = item.Description;
            _resourceTypeBox.SelectedItem = item.ResourceType;
            _pathBox.Text = item.Path;
            _iconBox.SelectedItem = item.IconName;
            _categoryBox.Text = item.Category;
            _colorPicker.SelectedColorHex = item.ColorHex;
            _sortOrderBox.Value = item.SortOrder;
        }
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

    private void OnBrowseClick(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Seleccionar archivo",
            Filter = "Todos los archivos (*.*)|*.*|Power BI (*.pbix)|*.pbix|Excel (*.xlsx;*.xls)|*.xlsx;*.xls"
        };
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            _pathBox.Text = dialog.FileName;

            // Auto-detect resource type from extension
            var ext = System.IO.Path.GetExtension(dialog.FileName).ToLowerInvariant();
            switch (ext)
            {
                case ".pbix":
                    _resourceTypeBox.SelectedItem = "PowerBI";
                    break;
                case ".xlsx" or ".xls":
                    _resourceTypeBox.SelectedItem = "Excel";
                    break;
            }
        }
    }

    private void OnSaveClick(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_titleBox.Text))
        {
            MessageBox.Show("El título es obligatorio.", "Validación",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_pathBox.Text))
        {
            MessageBox.Show("La ruta o URL es obligatoria.", "Validación",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ResultItem = _existingItem ?? new DirectoryItem();
        ResultItem.Title = _titleBox.Text.Trim();
        ResultItem.Description = _descriptionBox.Text.Trim();
        ResultItem.ResourceType = _resourceTypeBox.SelectedItem?.ToString() ?? "Other";
        ResultItem.Path = _pathBox.Text.Trim();
        ResultItem.IconName = _iconBox.SelectedItem?.ToString() ?? "chart";
        ResultItem.Category = _categoryBox.Text.Trim();
        ResultItem.ColorHex = _colorPicker.SelectedColorHex;
        ResultItem.SortOrder = (int)_sortOrderBox.Value;

        DialogResult = DialogResult.OK;
        Close();
    }
}
