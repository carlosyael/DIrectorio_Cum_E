using System.Drawing.Drawing2D;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class ColorPickerControl : UserControl
{
    private Color _selectedColor = Color.FromArgb(37, 99, 246); // Default #2563eb
    private readonly Panel _previewBox;
    private readonly TextBox _hexBox;
    private readonly FlowLayoutPanel _palettePanel;
    private bool _updatingInternally;

    public event EventHandler? ColorChanged;

    private static readonly string[] PresetColors =
    {
        "#3B82F6", // Azul Corporativo
        "#0284C7", // Azul Océano
        "#06B6D4", // Cian
        "#10B981", // Verde Esmeralda
        "#14B8A6", // Turquesa
        "#F59E0B", // Ámbar
        "#F97316", // Naranja
        "#EF4444", // Rojo Riesgo
        "#8B5CF6", // Violeta
        "#A855F7", // Púrpura
        "#EC4899", // Rosa
        "#64748B"  // Slate / Gris
    };

    public ColorPickerControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer, true);

        Height = 78;
        BackColor = Color.Transparent;

        // Top line: Preview box + Hex input + Custom palette button
        var topRow = new Panel
        {
            Dock = DockStyle.Top,
            Height = 36,
            BackColor = Color.Transparent
        };

        _previewBox = new Panel
        {
            Location = new Point(0, 0),
            Size = new Size(36, 36),
            BackColor = _selectedColor,
            Cursor = Cursors.Hand
        };
        _previewBox.Paint += (s, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, _previewBox.Width - 1, _previewBox.Height - 1);
            using var path = CreateRoundedRect(r, 8);
            using var brush = new SolidBrush(_selectedColor);
            g.FillPath(brush, path);
            using var pen = new Pen(DarkTheme.CardBorder, 1.5f);
            g.DrawPath(pen, path);
        };
        _previewBox.Click += (_, _) => OpenAdvancedColorDialog();

        _hexBox = new TextBox
        {
            Location = new Point(46, 3),
            Size = new Size(110, 30),
            Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextPrimary,
            BorderStyle = BorderStyle.FixedSingle,
            Text = ColorTranslator.ToHtml(_selectedColor)
        };
        _hexBox.TextChanged += OnHexBoxChanged;

        var customBtn = new Button
        {
            Text = "🎨 Paleta Personalizada...",
            Location = new Point(166, 2),
            Size = new Size(180, 32),
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.CardBackground,
            ForeColor = DarkTheme.TextPrimary,
            Font = new Font("Segoe UI", 9f),
            Cursor = Cursors.Hand
        };
        customBtn.FlatAppearance.BorderSize = 1;
        customBtn.FlatAppearance.BorderColor = DarkTheme.CardBorder;
        customBtn.Click += (_, _) => OpenAdvancedColorDialog();

        topRow.Controls.Add(_previewBox);
        topRow.Controls.Add(_hexBox);
        topRow.Controls.Add(customBtn);

        // Bottom row: Preset color swatches
        _palettePanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 34,
            BackColor = Color.Transparent,
            WrapContents = false,
            AutoScroll = false,
            Padding = new Padding(0, 4, 0, 0)
        };

        foreach (var hex in PresetColors)
        {
            var swatchColor = ColorTranslator.FromHtml(hex);
            var swatch = new Panel
            {
                Size = new Size(24, 24),
                BackColor = swatchColor,
                Margin = new Padding(0, 0, 8, 0),
                Cursor = Cursors.Hand,
                Tag = hex
            };
            swatch.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var r = new Rectangle(0, 0, swatch.Width - 1, swatch.Height - 1);
                using var path = CreateRoundedRect(r, 6);
                using var b = new SolidBrush(swatchColor);
                g.FillPath(b, path);

                bool isSelected = ColorTranslator.ToHtml(_selectedColor).Equals(hex, StringComparison.OrdinalIgnoreCase);
                using var p = new Pen(isSelected ? Color.White : Color.FromArgb(70, 0, 0, 0), isSelected ? 2f : 1f);
                g.DrawPath(p, path);
            };
            swatch.Click += (s, _) =>
            {
                if (s is Panel p && p.Tag is string cHex)
                {
                    SetColor(ColorTranslator.FromHtml(cHex));
                }
            };
            _palettePanel.Controls.Add(swatch);
        }

        Controls.Add(_palettePanel);
        Controls.Add(topRow);
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string SelectedColorHex
    {
        get => ColorTranslator.ToHtml(_selectedColor);
        set
        {
            try
            {
                SetColor(ColorTranslator.FromHtml(value));
            }
            catch
            {
                SetColor(Color.FromArgb(37, 99, 246));
            }
        }
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public Color SelectedColor
    {
        get => _selectedColor;
        set => SetColor(value);
    }

    public void SetColor(Color color)
    {
        _selectedColor = color;
        _previewBox.Invalidate();
        _palettePanel.Invalidate(true);

        if (!_updatingInternally)
        {
            _updatingInternally = true;
            _hexBox.Text = ColorTranslator.ToHtml(color);
            _updatingInternally = false;
        }

        ColorChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnHexBoxChanged(object? sender, EventArgs e)
    {
        if (_updatingInternally) return;

        var text = _hexBox.Text.Trim();
        if (!text.StartsWith("#")) text = "#" + text;

        try
        {
            var parsed = ColorTranslator.FromHtml(text);
            _updatingInternally = true;
            _selectedColor = parsed;
            _previewBox.Invalidate();
            _palettePanel.Invalidate(true);
            _updatingInternally = false;
            ColorChanged?.Invoke(this, EventArgs.Empty);
        }
        catch
        {
            // Ignore temporary typing states
        }
    }

    private void OpenAdvancedColorDialog()
    {
        using var dialog = new ColorDialog
        {
            Color = _selectedColor,
            FullOpen = true,
            AnyColor = true
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            SetColor(dialog.Color);
        }
    }

    private static GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
