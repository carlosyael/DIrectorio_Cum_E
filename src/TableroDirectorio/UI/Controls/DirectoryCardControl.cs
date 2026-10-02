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

        Margin = new Padding(12);
        Size = new Size(320, 295);
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
        _buttonRect = new Rectangle(20, Height - 54, Width - 40, 38);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (_item == null) return;

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        const int cardRadius = 14;
        var stripeColor = DarkTheme.GetCardColor(_item.ColorHex);

        using (var cardPath = CreateRoundedRectPath(rect, cardRadius))
        {
            // 1. Fill Card Background
            var bgColor = _isHovered ? DarkTheme.CardHover : DarkTheme.GetCardBackgroundWithTint(_item.ColorHex);
            using (var brush = new SolidBrush(bgColor))
            {
                g.FillPath(brush, cardPath);
            }

            // 2. Top Color Stripe with perfect clipping to rounded corners (no corner leakage)
            var oldClip = g.Clip;
            g.SetClip(cardPath);
            using (var stripeBrush = new SolidBrush(stripeColor))
            {
                g.FillRectangle(stripeBrush, 0, 0, Width, 5);
            }
            g.Clip = oldClip;

            // 3. Card Border
            var borderColor = _isHovered ? stripeColor : DarkTheme.CardBorder;
            using (var pen = new Pen(borderColor, _isHovered ? 1.5f : 1f))
            {
                g.DrawPath(pen, cardPath);
            }
        }

        // 4. Icon Container & Vector Icon
        var iconBoxRect = new Rectangle(20, 18, 46, 46);
        using (var iconBg = new SolidBrush(Color.FromArgb(ThemeManager.Current.IsDark ? 28 : 16, stripeColor.R, stripeColor.G, stripeColor.B)))
        {
            using (var iconPath = CreateRoundedRectPath(iconBoxRect, 10))
            {
                g.FillPath(iconBg, iconPath);
                using var iconPen = new Pen(Color.FromArgb(60, stripeColor.R, stripeColor.G, stripeColor.B), 1f);
                g.DrawPath(iconPen, iconPath);
            }
        }

        var iconInner = new Rectangle(iconBoxRect.X + 8, iconBoxRect.Y + 8, iconBoxRect.Width - 16, iconBoxRect.Height - 16);
        IconRenderer.DrawIcon(g, _item.IconName, iconInner, stripeColor);

        // 5. Category (Top Right)
        if (!string.IsNullOrWhiteSpace(_item.Category))
        {
            var catRect = new Rectangle(Width - 140, 22, 120, 20);
            TextRenderer.DrawText(g, _item.Category, new Font("Segoe UI", 9f, FontStyle.Regular),
                catRect, DarkTheme.TextMuted, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
        }

        // 6. Title
        var titleRect = new Rectangle(20, 74, Width - 40, 48);
        TextRenderer.DrawText(g, _item.Title, new Font("Segoe UI", 11.5f, FontStyle.Bold), titleRect,
            DarkTheme.TextPrimary,
            TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.EndEllipsis);

        // 7. Description
        var descRect = new Rectangle(20, 126, Width - 40, 50);
        TextRenderer.DrawText(g, _item.Description, new Font("Segoe UI", 9f, FontStyle.Regular), descRect,
            DarkTheme.TextSecondary,
            TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.EndEllipsis);

        // 8. Technology Badge (Pill)
        var badgeText = _item.ResourceType;
        var badgeFont = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        var badgeSize = TextRenderer.MeasureText(badgeText, badgeFont);
        int badgeY = Height - 54 - badgeSize.Height - 14;
        var badgeRect = new Rectangle(20, badgeY, badgeSize.Width + 14, badgeSize.Height + 5);

        using (var badgePath = CreateRoundedRectPath(badgeRect, 5))
        {
            using var badgeBg = new SolidBrush(Color.FromArgb(ThemeManager.Current.IsDark ? 32 : 18, stripeColor.R, stripeColor.G, stripeColor.B));
            g.FillPath(badgeBg, badgePath);
            using var badgePen = new Pen(Color.FromArgb(80, stripeColor.R, stripeColor.G, stripeColor.B), 1f);
            g.DrawPath(badgePen, badgePath);
        }
        TextRenderer.DrawText(g, badgeText, badgeFont, badgeRect,
            stripeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        // 9. Open Button (Modern Rounded Action)
        _buttonRect = new Rectangle(20, Height - 50, Width - 40, 36);
        using (var btnPath = CreateRoundedRectPath(_buttonRect, 8))
        {
            var btnBg = _isButtonHovered
                ? Color.FromArgb(Math.Min(255, DarkTheme.AccentBlue.R + 20), Math.Min(255, DarkTheme.AccentBlue.G + 20), Math.Min(255, DarkTheme.AccentBlue.B + 20))
                : DarkTheme.AccentBlue;

            using var btnBrush = new SolidBrush(btnBg);
            g.FillPath(btnBrush, btnPath);
        }

        TextRenderer.DrawText(g, "Abrir Tablero", new Font("Segoe UI", 9.5f, FontStyle.Bold), _buttonRect,
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
}
