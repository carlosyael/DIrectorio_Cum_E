using System.Drawing.Drawing2D;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class ThemeToggleControl : UserControl
{
    private bool _isHovered;

    public event EventHandler? ThemeToggled;

    public ThemeToggleControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);

        Size = new Size(130, 32);
        Cursor = Cursors.Hand;
        BackColor = Color.Transparent;

        MouseEnter += (_, _) => { _isHovered = true; Invalidate(); };
        MouseLeave += (_, _) => { _isHovered = false; Invalidate(); };
        Click += (_, _) =>
        {
            ThemeManager.ToggleTheme();
            ThemeToggled?.Invoke(this, EventArgs.Empty);
            Invalidate();
        };

        ThemeManager.ThemeChanged += Invalidate;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        const int radius = 8;
        bool isDark = ThemeManager.IsDark;

        // Background & border
        var bg = _isHovered ? DarkTheme.CardHover : DarkTheme.CardBackground;
        using (var path = CreateRoundedRect(rect, radius))
        {
            using var brush = new SolidBrush(bg);
            g.FillPath(brush, path);

            using var pen = new Pen(_isHovered ? DarkTheme.AccentBlue : DarkTheme.CardBorder, 1f);
            g.DrawPath(pen, path);
        }

        // Vector Icon (Sun if dark -> click gives light; Moon if light -> click gives dark)
        var iconRect = new Rectangle(8, 6, 20, 20);
        var iconColor = isDark ? DarkTheme.AccentSky : DarkTheme.AccentBlue;
        if (isDark)
        {
            IconRenderer.DrawSun(g, iconRect, iconColor);
        }
        else
        {
            IconRenderer.DrawMoon(g, iconRect, iconColor);
        }

        // Mode Text
        var text = isDark ? "Modo Claro" : "Modo Oscuro";
        var textRect = new Rectangle(32, 0, Width - 36, Height);
        using var font = new Font("Segoe UI", 9.2f, FontStyle.Regular);
        TextRenderer.DrawText(g, text, font, textRect,
            DarkTheme.TextPrimary, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
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
