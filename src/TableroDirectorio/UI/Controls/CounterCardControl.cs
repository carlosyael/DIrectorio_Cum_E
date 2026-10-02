using System.Drawing.Drawing2D;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class CounterCardControl : UserControl
{
    private int _count;

    public CounterCardControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);

        Size = new Size(230, 96);
        BackColor = Color.Transparent;
        ThemeManager.ThemeChanged += Invalidate;
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        const int radius = 14;

        using (var path = CreateRoundedRect(rect, radius))
        {
            var isDark = ThemeManager.Current.IsDark;
            var bgColor = isDark
                ? Color.FromArgb(28, 38, 54)
                : Color.White;

            using var bgBrush = new SolidBrush(bgColor);
            g.FillPath(bgBrush, path);

            var borderColor = isDark
                ? Color.FromArgb(60, 75, 100)
                : Color.FromArgb(226, 232, 240);

            using var pen = new Pen(borderColor, 1.2f);
            g.DrawPath(pen, path);
        }

        // Count Number (With ample vertical space and NoClipping to ensure full digit display)
        var numRect = new Rectangle(0, 6, Width, 54);
        using var numFont = new Font("Segoe UI", 28f, FontStyle.Bold);
        TextRenderer.DrawText(g, _count.ToString(), numFont,
            numRect, DarkTheme.AccentBlue,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoClipping);

        // Subtitle Text
        var textRect = new Rectangle(0, 60, Width, 26);
        using var textFont = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        TextRenderer.DrawText(g, "Tableros Disponibles", textFont,
            textRect, DarkTheme.TextMuted,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.NoClipping);
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
