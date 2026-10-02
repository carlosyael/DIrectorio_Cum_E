using System.Drawing.Drawing2D;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class SearchBoxControl : UserControl
{
    private readonly TextBox _innerBox;
    private const string Placeholder = "Buscar tablero por título, descripción o categoría...";
    private bool _isPlaceholder = true;

    public event EventHandler? SearchTextChanged;

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Query => _isPlaceholder ? string.Empty : _innerBox.Text.Trim();

    public SearchBoxControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);

        Height = 46;
        MinimumSize = new Size(320, 46);
        BackColor = Color.Transparent;
        Padding = new Padding(46, 12, 16, 10);

        _innerBox = new TextBox
        {
            BorderStyle = BorderStyle.None,
            Font = new Font("Segoe UI", 11f, FontStyle.Regular),
            Dock = DockStyle.Fill,
            Text = Placeholder,
            BackColor = DarkTheme.InputBackground,
            ForeColor = DarkTheme.TextMuted
        };

        _innerBox.GotFocus += (_, _) =>
        {
            if (_isPlaceholder)
            {
                _innerBox.Text = "";
                _innerBox.ForeColor = DarkTheme.TextPrimary;
                _isPlaceholder = false;
            }
        };

        _innerBox.LostFocus += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(_innerBox.Text))
            {
                _innerBox.Text = Placeholder;
                _innerBox.ForeColor = DarkTheme.TextMuted;
                _isPlaceholder = true;
            }
        };

        _innerBox.TextChanged += (_, e) =>
        {
            if (!_isPlaceholder)
            {
                SearchTextChanged?.Invoke(this, e);
            }
        };

        Controls.Add(_innerBox);
        ThemeManager.ThemeChanged += ApplyTheme;
    }

    public void ApplyTheme()
    {
        _innerBox.BackColor = DarkTheme.InputBackground;
        _innerBox.ForeColor = _isPlaceholder ? DarkTheme.TextMuted : DarkTheme.TextPrimary;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        const int radius = 12;

        using (var path = CreateRoundedPath(rect, radius))
        {
            using var brush = new SolidBrush(DarkTheme.InputBackground);
            g.FillPath(brush, path);

            using var pen = new Pen(DarkTheme.InputBorder, 1.2f);
            g.DrawPath(pen, path);
        }

        // Draw clean vector search lens icon on the left
        var iconRect = new Rectangle(14, 11, 24, 24);
        IconRenderer.DrawSearch(g, iconRect, DarkTheme.TextMuted);
    }

    private static GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
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
