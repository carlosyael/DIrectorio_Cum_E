using System.Drawing.Drawing2D;
using TableroDirectorio.Models;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class DirectoryCardControl : UserControl
{
    private DirectoryItem _item = null!;
    private bool _isHovered;
    private bool _isButtonHovered;
    private Rectangle _buttonRect;

    public event EventHandler<DirectoryItem>? CardClicked;

    public DirectoryCardControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);

        Margin = new Padding(14);
        Size = new Size(330, 390);
        Cursor = Cursors.Hand;

        MouseEnter += (_, _) => { _isHovered = true; Invalidate(); };
        MouseLeave += (_, _) => { _isHovered = false; _isButtonHovered = false; Invalidate(); };
        MouseMove += OnCardMouseMove;
        MouseClick += OnCardMouseClick;
    }

    public void SetItem(DirectoryItem item)
    {
        _item = item;
        Invalidate();
    }

    private void OnCardMouseMove(object? sender, MouseEventArgs e)
    {
        bool wasBtnHovered = _isButtonHovered;
        _isButtonHovered = _buttonRect.Contains(e.Location);
        if (wasBtnHovered != _isButtonHovered)
        {
            Invalidate();
        }
    }

    private void OnCardMouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && _item != null)
        {
            CardClicked?.Invoke(this, _item);
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        _buttonRect = new Rectangle(24, Height - 60, Width - 48, 42);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (_item == null) return;

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        const int cardRadius = 18;

        // 1. Card Body with glass/matte tint
        var bgColor = _isHovered ? DarkTheme.CardHover : DarkTheme.GetCardBackgroundWithTint(_item.ColorHex);
        using (var path = CreateRoundedRectPath(rect, cardRadius))
        {
            using var brush = new SolidBrush(bgColor);
            g.FillPath(brush, path);

            var borderColor = _isHovered ? DarkTheme.AccentSky : DarkTheme.CardBorder;
            using var pen = new Pen(borderColor, _isHovered ? 1.5f : 1f);
            g.DrawPath(pen, path);
        }

        // 2. Top Accent Line (following top curve)
        var stripeColor = DarkTheme.GetCardColor(_item.ColorHex);
        using (var stripePath = CreateTopStripePath(new Rectangle(0, 0, Width - 1, 6), cardRadius))
        {
            using var stripeBrush = new SolidBrush(stripeColor);
            g.FillPath(stripeBrush, stripePath);
        }

        // 3. Icon Container & Vector Icon
        var iconBoxRect = new Rectangle(24, 22, 54, 54);
        using (var iconBg = new SolidBrush(Color.FromArgb(28, 255, 255, 255)))
        {
            using var iconPath = CreateRoundedRectPath(iconBoxRect, 14);
            g.FillPath(iconBg, iconPath);
            using var iconBorder = new Pen(Color.FromArgb(40, stripeColor.R, stripeColor.G, stripeColor.B), 1.2f);
            g.DrawPath(iconBorder, iconPath);
        }

        var iconInner = new Rectangle(iconBoxRect.X + 8, iconBoxRect.Y + 8, iconBoxRect.Width - 16, iconBoxRect.Height - 16);
        IconRenderer.DrawIcon(g, _item.IconName, iconInner, stripeColor);

        // 4. Category Tag (top right)
        if (!string.IsNullOrWhiteSpace(_item.Category))
        {
            var catRect = new Rectangle(Width - 150, 26, 126, 22);
            TextRenderer.DrawText(g, _item.Category, new Font("Segoe UI", 9.2f, FontStyle.Regular),
                catRect, DarkTheme.TextMuted, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
        }

        // 5. Title (with generous vertical bounds)
        var titleRect = new Rectangle(24, 90, Width - 48, 58);
        TextRenderer.DrawText(g, _item.Title, DarkTheme.CardTitleFont, titleRect,
            DarkTheme.TextPrimary,
            TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.EndEllipsis);

        // 6. Description (clearly separated)
        var descRect = new Rectangle(24, 154, Width - 48, 80);
        TextRenderer.DrawText(g, _item.Description, DarkTheme.CardBodyFont, descRect,
            DarkTheme.TextSecondary,
            TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.EndEllipsis);

        // 7. Technology Badge (Pill style, well above button)
        var badgeText = _item.ResourceType;
        var badgeSize = TextRenderer.MeasureText(badgeText, new Font("Segoe UI", 8.8f, FontStyle.Bold));
        int badgeY = Height - 60 - badgeSize.Height - 16;
        var badgeRect = new Rectangle(24, badgeY, badgeSize.Width + 16, badgeSize.Height + 6);

        using (var badgePath = CreateRoundedRectPath(badgeRect, 6))
        {
            using var badgeBg = new SolidBrush(Color.FromArgb(32, stripeColor.R, stripeColor.G, stripeColor.B));
            g.FillPath(badgeBg, badgePath);
            using var badgePen = new Pen(Color.FromArgb(90, stripeColor.R, stripeColor.G, stripeColor.B), 1f);
            g.DrawPath(badgePen, badgePath);
        }

        TextRenderer.DrawText(g, badgeText, new Font("Segoe UI", 8.8f, FontStyle.Bold), badgeRect,
            stripeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        // 8. Open Button (Modern Rounded with subtle gradient/state)
        _buttonRect = new Rectangle(24, Height - 58, Width - 48, 42);
        using (var btnPath = CreateRoundedRectPath(_buttonRect, 10))
        {
            var btnColor = _isButtonHovered
                ? Color.FromArgb(
                    Math.Min(255, DarkTheme.AccentBlue.R + 25),
                    Math.Min(255, DarkTheme.AccentBlue.G + 25),
                    Math.Min(255, DarkTheme.AccentBlue.B + 25))
                : DarkTheme.AccentBlue;

            using var btnBrush = new SolidBrush(btnColor);
            g.FillPath(btnBrush, btnPath);
        }

        TextRenderer.DrawText(g, "Abrir Tablero", DarkTheme.ButtonFont, _buttonRect,
            Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    private static GraphicsPath CreateRoundedRectPath(Rectangle rect, int radius)
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

    private static GraphicsPath CreateTopStripePath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
        path.CloseFigure();
        return path;
    }
}
