using System.Drawing.Drawing2D;
using TableroDirectorio.Models;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class DirectoryCardControl : UserControl
{
    private DirectoryItem _item = null!;
    private bool _isHovered;
    private readonly Button _openButton;

    public event EventHandler<DirectoryItem>? CardClicked;

    public DirectoryCardControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);

        Margin = new Padding(15);
        Cursor = Cursors.Hand;

        _openButton = CreateStyledButton("Abrir Tablero");
        _openButton.Click += (_, _) => CardClicked?.Invoke(this, _item);
        Controls.Add(_openButton);

        Size = new Size(330, 270);

        MouseEnter += (_, _) => { _isHovered = true; Invalidate(); };
        MouseLeave += (_, _) => { _isHovered = false; Invalidate(); };
        Click += (_, _) => CardClicked?.Invoke(this, _item);
    }

    public void SetItem(DirectoryItem item)
    {
        _item = item;
        Invalidate();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (_openButton != null)
        {
            _openButton.SetBounds(20, Height - 52, Width - 40, 38);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (_item == null) return;

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        const int radius = 18;

        // Background
        var bgColor = _isHovered ? DarkTheme.CardHover : DarkTheme.GetCardBackgroundWithTint(_item.ColorHex);
        using (var path = CreateRoundedRectPath(rect, radius))
        {
            using var brush = new SolidBrush(bgColor);
            g.FillPath(brush, path);

            using var pen = new Pen(DarkTheme.CardBorder, 1f);
            g.DrawPath(pen, path);
        }

        // Top accent line
        var stripeColor = DarkTheme.GetCardColor(_item.ColorHex);
        using (var stripePath = CreateTopStripePath(new Rectangle(0, 0, Width, 5), radius))
        {
            using var stripeBrush = new SolidBrush(stripeColor);
            g.FillPath(stripeBrush, stripePath);
        }

        // Icon box
        var iconText = DarkTheme.GetIcon(_item.IconName);
        var iconRect = new Rectangle(20, 20, 50, 50);
        using (var iconBg = new SolidBrush(Color.FromArgb(28, 255, 255, 255)))
        {
            using var iconPath = CreateRoundedRectPath(iconRect, 12);
            g.FillPath(iconBg, iconPath);
        }
        TextRenderer.DrawText(g, iconText, new Font("Segoe UI Emoji", 20f), iconRect,
            DarkTheme.TextPrimary, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        // Category Tag (if present)
        if (!string.IsNullOrWhiteSpace(_item.Category))
        {
            var catRect = new Rectangle(Width - 130, 24, 110, 24);
            TextRenderer.DrawText(g, _item.Category, DarkTheme.CardBodyFont, catRect,
                DarkTheme.TextMuted, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
        }

        // Title
        var titleRect = new Rectangle(20, 80, Width - 40, 52);
        TextRenderer.DrawText(g, _item.Title, DarkTheme.CardTitleFont, titleRect,
            DarkTheme.TextPrimary,
            TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.Top);

        // Description
        var descRect = new Rectangle(20, 136, Width - 40, 50);
        TextRenderer.DrawText(g, _item.Description, DarkTheme.CardBodyFont, descRect,
            DarkTheme.TextSecondary,
            TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.Top);

        // Resource type badge
        var badgeText = _item.ResourceType;
        var badgeSize = TextRenderer.MeasureText(badgeText, DarkTheme.CardBodyFont);
        var badgeRect = new Rectangle(20, Height - 60 - badgeSize.Height, badgeSize.Width + 14, badgeSize.Height + 4);
        using (var badgePath = CreateRoundedRectPath(badgeRect, 6))
        {
            using var badgeBrush = new SolidBrush(Color.FromArgb(35, stripeColor.R, stripeColor.G, stripeColor.B));
            g.FillPath(badgeBrush, badgePath);
        }
        TextRenderer.DrawText(g, badgeText, DarkTheme.CardBodyFont, badgeRect,
            stripeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        // Update button color dynamically
        _openButton.BackColor = DarkTheme.AccentBlue;
    }

    private static Button CreateStyledButton(string text)
    {
        var btn = new Button
        {
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = DarkTheme.AccentBlue,
            ForeColor = Color.White,
            Font = DarkTheme.ButtonFont,
            Cursor = Cursors.Hand,
            Height = 38
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }

    private static GraphicsPath CreateRoundedRectPath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        var d = radius * 2;
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
        var d = radius * 2;
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
        path.CloseFigure();
        return path;
    }
}
