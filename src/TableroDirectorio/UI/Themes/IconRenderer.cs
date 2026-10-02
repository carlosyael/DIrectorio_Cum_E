using System.Drawing.Drawing2D;

namespace TableroDirectorio.UI.Themes;

/// <summary>
/// Provee renderizado vectorial de iconos corporativos limpios mediante GDI+,
/// garantizando nitidez perfecta en cualquier pantalla sin depender de fuentes de emojis.
/// </summary>
public static class IconRenderer
{
    public static void DrawIcon(Graphics g, string iconName, Rectangle bounds, Color color)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var name = iconName?.ToLowerInvariant() ?? "";
        switch (name)
        {
            case "warning" or "triangle-exclamation" or "riesgos":
                DrawWarning(g, bounds, color);
                break;
            case "calendar" or "calendar-days" or "planificación":
                DrawCalendar(g, bounds, color);
                break;
            case "search" or "magnifying-glass-chart" or "evaluación":
                DrawSearch(g, bounds, color);
                break;
            case "file-excel" or "excel":
                DrawExcel(g, bounds, color);
                break;
            case "globe" or "webpage" or "web":
                DrawGlobe(g, bounds, color);
                break;
            case "gear" or "settings" or "config":
                DrawGear(g, bounds, color);
                break;
            case "palette" or "theme":
                DrawPalette(g, bounds, color);
                break;
            case "shield":
                DrawShield(g, bounds, color);
                break;
            case "chart-line" or "chart" or "indicadores":
            default:
                DrawChart(g, bounds, color);
                break;
        }
    }

    public static void DrawShield(Graphics g, Rectangle bounds, Color color)
    {
        int p = bounds.Width / 6;
        var r = new Rectangle(bounds.X + p, bounds.Y + p, bounds.Width - 2 * p, bounds.Height - 2 * p);

        using var path = new GraphicsPath();
        path.AddLine(r.Left, r.Top, r.Right, r.Top);
        path.AddLine(r.Right, r.Top, r.Right, r.Top + r.Height * 0.45f);
        path.AddBezier(
            r.Right, r.Top + r.Height * 0.45f,
            r.Right - r.Width * 0.1f, r.Bottom - r.Height * 0.1f,
            r.Left + r.Width * 0.5f, r.Bottom,
            r.Left + r.Width * 0.5f, r.Bottom);
        path.AddBezier(
            r.Left + r.Width * 0.5f, r.Bottom,
            r.Left + r.Width * 0.1f, r.Bottom - r.Height * 0.1f,
            r.Left, r.Top + r.Height * 0.45f,
            r.Left, r.Top + r.Height * 0.45f);
        path.CloseFigure();

        using var pen = new Pen(color, 2.2f) { LineJoin = LineJoin.Round };
        g.DrawPath(pen, path);

        // Subtle inner fill or emblem line
        using var fillBrush = new SolidBrush(Color.FromArgb(40, color.R, color.G, color.B));
        g.FillPath(fillBrush, path);
    }

    public static void DrawChart(Graphics g, Rectangle bounds, Color color)
    {
        int p = bounds.Width / 5;
        var r = new Rectangle(bounds.X + p, bounds.Y + p, bounds.Width - 2 * p, bounds.Height - 2 * p);

        using var pen = new Pen(color, 2.2f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        // Base line
        g.DrawLine(pen, r.Left, r.Bottom, r.Right, r.Bottom);

        // 3 Bars
        int barW = r.Width / 5;
        int gap = (r.Width - 3 * barW) / 4;

        using var brush = new SolidBrush(color);
        // Bar 1
        int h1 = (int)(r.Height * 0.45f);
        g.FillRectangle(brush, r.Left + gap, r.Bottom - h1, barW, h1);

        // Bar 2
        int h2 = (int)(r.Height * 0.85f);
        g.FillRectangle(brush, r.Left + gap * 2 + barW, r.Bottom - h2, barW, h2);

        // Bar 3
        int h3 = (int)(r.Height * 0.65f);
        g.FillRectangle(brush, r.Left + gap * 3 + barW * 2, r.Bottom - h3, barW, h3);
    }

    public static void DrawWarning(Graphics g, Rectangle bounds, Color color)
    {
        int p = bounds.Width / 6;
        var r = new Rectangle(bounds.X + p, bounds.Y + p, bounds.Width - 2 * p, bounds.Height - 2 * p);

        using var path = new GraphicsPath();
        path.AddLine(r.Left + r.Width / 2f, r.Top, r.Right, r.Bottom);
        path.AddLine(r.Right, r.Bottom, r.Left, r.Bottom);
        path.CloseFigure();

        using var pen = new Pen(color, 2.2f) { LineJoin = LineJoin.Round };
        g.DrawPath(pen, path);

        using var brush = new SolidBrush(color);
        // Exclamation mark
        int midX = r.Left + r.Width / 2;
        g.DrawLine(pen, midX, r.Top + (int)(r.Height * 0.38f), midX, r.Bottom - (int)(r.Height * 0.35f));
        g.FillEllipse(brush, midX - 1.5f, r.Bottom - (int)(r.Height * 0.22f), 3f, 3f);
    }

    public static void DrawCalendar(Graphics g, Rectangle bounds, Color color)
    {
        int p = bounds.Width / 6;
        var r = new Rectangle(bounds.X + p, bounds.Y + p, bounds.Width - 2 * p, bounds.Height - 2 * p);

        using var pen = new Pen(color, 2f);
        g.DrawRectangle(pen, r);
        g.DrawLine(pen, r.Left, r.Top + r.Height * 0.32f, r.Right, r.Top + r.Height * 0.32f);

        // Binder rings
        g.DrawLine(pen, r.Left + r.Width * 0.25f, r.Top - 3, r.Left + r.Width * 0.25f, r.Top + 3);
        g.DrawLine(pen, r.Right - r.Width * 0.25f, r.Top - 3, r.Right - r.Width * 0.25f, r.Top + 3);

        // Day dots
        using var brush = new SolidBrush(color);
        int dotY = (int)(r.Top + r.Height * 0.55f);
        int dotSize = Math.Max(2, r.Width / 10);
        for (int i = 1; i <= 3; i++)
        {
            int dotX = (int)(r.Left + r.Width * (i * 0.25f) - dotSize / 2f);
            g.FillEllipse(brush, dotX, dotY, dotSize, dotSize);
        }
    }

    public static void DrawSearch(Graphics g, Rectangle bounds, Color color)
    {
        int p = bounds.Width / 5;
        var r = new Rectangle(bounds.X + p, bounds.Y + p, bounds.Width - 2 * p, bounds.Height - 2 * p);

        using var pen = new Pen(color, 2.2f);
        int circleSize = (int)(r.Width * 0.65f);
        g.DrawEllipse(pen, r.Left, r.Top, circleSize, circleSize);

        // Handle
        float hx1 = r.Left + circleSize * 0.85f;
        float hy1 = r.Top + circleSize * 0.85f;
        g.DrawLine(pen, hx1, hy1, r.Right, r.Bottom);
    }

    public static void DrawExcel(Graphics g, Rectangle bounds, Color color)
    {
        int p = bounds.Width / 6;
        var r = new Rectangle(bounds.X + p, bounds.Y + p, bounds.Width - 2 * p, bounds.Height - 2 * p);

        using var pen = new Pen(color, 2f);
        g.DrawRectangle(pen, r);
        g.DrawLine(pen, r.Left, r.Top + r.Height * 0.5f, r.Right, r.Top + r.Height * 0.5f);
        g.DrawLine(pen, r.Left + r.Width * 0.5f, r.Top, r.Left + r.Width * 0.5f, r.Bottom);
    }

    public static void DrawGlobe(Graphics g, Rectangle bounds, Color color)
    {
        int p = bounds.Width / 6;
        var r = new Rectangle(bounds.X + p, bounds.Y + p, bounds.Width - 2 * p, bounds.Height - 2 * p);

        using var pen = new Pen(color, 2f);
        g.DrawEllipse(pen, r);
        g.DrawLine(pen, r.Left, r.Top + r.Height / 2, r.Right, r.Top + r.Height / 2);
        g.DrawEllipse(pen, r.Left + r.Width * 0.22f, r.Top, r.Width * 0.56f, r.Height);
    }

    public static void DrawGear(Graphics g, Rectangle bounds, Color color)
    {
        int p = bounds.Width / 6;
        var r = new Rectangle(bounds.X + p, bounds.Y + p, bounds.Width - 2 * p, bounds.Height - 2 * p);

        using var pen = new Pen(color, 2f);
        g.DrawEllipse(pen, r.Left + r.Width * 0.25f, r.Top + r.Height * 0.25f, r.Width * 0.5f, r.Height * 0.5f);

        // 4 teeth crosses
        int cx = r.Left + r.Width / 2;
        int cy = r.Top + r.Height / 2;
        int reach = r.Width / 2;
        g.DrawLine(pen, cx, cy - reach, cx, cy + reach);
        g.DrawLine(pen, cx - reach, cy, cx + reach, cy);
    }

    public static void DrawPalette(Graphics g, Rectangle bounds, Color color)
    {
        int p = bounds.Width / 6;
        var r = new Rectangle(bounds.X + p, bounds.Y + p, bounds.Width - 2 * p, bounds.Height - 2 * p);

        using var pen = new Pen(color, 2f);
        g.DrawEllipse(pen, r);

        using var brush = new SolidBrush(color);
        g.FillEllipse(brush, r.Left + r.Width * 0.3f, r.Top + r.Height * 0.3f, 3f, 3f);
        g.FillEllipse(brush, r.Left + r.Width * 0.65f, r.Top + r.Height * 0.35f, 3f, 3f);
        g.FillEllipse(brush, r.Left + r.Width * 0.45f, r.Top + r.Height * 0.65f, 3f, 3f);
    }
}
