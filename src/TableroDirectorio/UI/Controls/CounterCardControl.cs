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

        Size = new Size(220, 96);
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
        const int radius = 18;

        using (var path = CreateRoundedRect(rect, radius))
        {
            using var bgBrush = new SolidBrush(Color.FromArgb(32, DarkTheme.TextPrimary.R, DarkTheme.TextPrimary.G, DarkTheme.TextPrimary.B));
            g.FillPath(bgBrush, path);

            using var pen = new Pen(Color.FromArgb(45, DarkTheme.TextPrimary.R, DarkTheme.TextPrimary.G, DarkTheme.TextPrimary.B), 1.2f);
            g.DrawPath(pen, path);
        }

        // Count Number
        var numRect = new Rectangle(0, 10, Width, 46);
        TextRenderer.DrawText(g, _count.ToString(), new Font("Segoe UI", 30f, FontStyle.Bold),
            numRect, DarkTheme.AccentSky, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        // Subtitle Text
        var textRect = new Rectangle(0, 56, Width, 30);
        TextRenderer.DrawText(g, "Tableros Disponibles", new Font("Segoe UI", 9.5f, FontStyle.Regular),
            textRect, DarkTheme.TextMuted, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
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
