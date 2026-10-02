using System.Drawing.Drawing2D;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class BannerPillControl : UserControl
{
    private string _bannerText = string.Empty;

    public BannerPillControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);

        Height = 36;
        BackColor = Color.Transparent;
        ThemeManager.ThemeChanged += Invalidate;
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string BannerText
    {
        get => _bannerText;
        set
        {
            _bannerText = value ?? "";
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (string.IsNullOrWhiteSpace(_bannerText)) return;

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        var size = TextRenderer.MeasureText(_bannerText, font);
        int pillW = size.Width + 36;
        int pillH = 30;
        int pillX = (Width - pillW) / 2;
        int pillY = (Height - pillH) / 2;

        var rect = new Rectangle(pillX, pillY, pillW, pillH);
        int radius = pillH / 2;

        using (var path = CreatePillPath(rect, radius))
        {
            var isDark = ThemeManager.Current.IsDark;
            var bgColor = isDark
                ? Color.FromArgb(25, 255, 255, 255)
                : Color.FromArgb(240, 244, 248);

            using var bg = new SolidBrush(bgColor);
            g.FillPath(bg, path);

            var borderColor = isDark
                ? Color.FromArgb(45, 255, 255, 255)
                : Color.FromArgb(210, 220, 230);

            using var pen = new Pen(borderColor, 1f);
            g.DrawPath(pen, path);
        }

        TextRenderer.DrawText(g, _bannerText, font, rect,
            DarkTheme.TextSecondary, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    private static GraphicsPath CreatePillPath(Rectangle rect, int radius)
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
