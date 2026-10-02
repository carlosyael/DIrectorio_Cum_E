using System.Drawing.Drawing2D;
using TableroDirectorio.Models;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class DirectoryCardControl : UserControl
{
    private DirectoryItem _item = null!;
    private bool _isHovered;
    private readonly Button _openButton;
    private readonly Button _editButton;
    private readonly Button _deleteButton;

    public event EventHandler<DirectoryItem>? CardClicked;
    public event EventHandler<DirectoryItem>? EditRequested;
    public event EventHandler<DirectoryItem>? DeleteRequested;

    public DirectoryCardControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);

        Margin = new Padding(15);
        Cursor = Cursors.Hand;

        // Open button
        _openButton = CreateStyledButton("Abrir Tablero", DarkTheme.AccentBlue);
        _openButton.Click += (_, _) => CardClicked?.Invoke(this, _item);

        // Edit button (small)
        _editButton = CreateSmallButton("✏️");
        _editButton.Click += (_, _) => EditRequested?.Invoke(this, _item);

        // Delete button (small)
        _deleteButton = CreateSmallButton("🗑️");
        _deleteButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 239, 68, 68);
        _deleteButton.Click += (_, _) => DeleteRequested?.Invoke(this, _item);

        Controls.Add(_openButton);
        Controls.Add(_editButton);
        Controls.Add(_deleteButton);

        // Set Size AFTER buttons exist so OnResize -> LayoutButtons doesn't hit nulls
        Size = new Size(330, 280);

        MouseEnter += (_, _) => { _isHovered = true; Invalidate(); };
        MouseLeave += (_, _) => { _isHovered = false; Invalidate(); };
    }

    public void SetItem(DirectoryItem item)
    {
        _item = item;
        Invalidate();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        LayoutButtons();
    }

    private void LayoutButtons()
    {
        if (_openButton == null || _editButton == null || _deleteButton == null)
            return;

        var btnWidth = Width - 40;
        _openButton.SetBounds(20, Height - 55, btnWidth, 38);
        _editButton.SetBounds(Width - 75, 15, 28, 28);
        _deleteButton.SetBounds(Width - 42, 15, 28, 28);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (_item == null) return;

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        var radius = 20;

        // Card background with color tint
        var bgColor = _isHovered ? DarkTheme.CardHover : DarkTheme.GetCardBackgroundWithTint(_item.ColorHex);
        using (var path = CreateRoundedRectPath(rect, radius))
        {
            using var brush = new SolidBrush(bgColor);
            g.FillPath(brush, path);

            using var pen = new Pen(DarkTheme.CardBorder, 1f);
            g.DrawPath(pen, path);
        }

        // Top color stripe
        var stripeColor = DarkTheme.GetCardColor(_item.ColorHex);
        using (var stripePath = CreateTopStripePath(new Rectangle(0, 0, Width, 6), radius))
        {
            using var stripeBrush = new SolidBrush(stripeColor);
            g.FillPath(stripeBrush, stripePath);
        }

        // Icon
        var iconText = DarkTheme.GetIcon(_item.IconName);
        var iconRect = new Rectangle(20, 25, 55, 55);
        using (var iconBg = new SolidBrush(Color.FromArgb(30, 255, 255, 255)))
        {
            using var iconPath = CreateRoundedRectPath(iconRect, 14);
            g.FillPath(iconBg, iconPath);
        }
        TextRenderer.DrawText(g, iconText, new Font("Segoe UI Emoji", 22f), iconRect,
            DarkTheme.TextPrimary, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        // Title
        var titleRect = new Rectangle(20, 90, Width - 40, 55);
        TextRenderer.DrawText(g, _item.Title, DarkTheme.CardTitleFont, titleRect,
            DarkTheme.TextPrimary,
            TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.Top);

        // Description
        var descRect = new Rectangle(20, 148, Width - 40, 60);
        TextRenderer.DrawText(g, _item.Description, DarkTheme.CardBodyFont, descRect,
            DarkTheme.TextSecondary,
            TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.Top);

        // Resource type badge
        var badgeText = _item.ResourceType;
        var badgeSize = TextRenderer.MeasureText(badgeText, DarkTheme.CardBodyFont);
        var badgeRect = new Rectangle(20, Height - 65 - badgeSize.Height - 8,
            badgeSize.Width + 16, badgeSize.Height + 6);
        using (var badgePath = CreateRoundedRectPath(badgeRect, 8))
        {
            using var badgeBrush = new SolidBrush(Color.FromArgb(40, stripeColor.R, stripeColor.G, stripeColor.B));
            g.FillPath(badgeBrush, badgePath);
        }
        TextRenderer.DrawText(g, badgeText, DarkTheme.CardBodyFont, badgeRect,
            stripeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    private static Button CreateStyledButton(string text, Color accentColor)
    {
        var btn = new Button
        {
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = accentColor,
            ForeColor = Color.White,
            Font = DarkTheme.ButtonFont,
            Cursor = Cursors.Hand,
            Height = 38,
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(
            Math.Min(255, accentColor.R + 30),
            Math.Min(255, accentColor.G + 30),
            Math.Min(255, accentColor.B + 30));
        return btn;
    }

    private static Button CreateSmallButton(string text)
    {
        var btn = new Button
        {
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Transparent,
            ForeColor = DarkTheme.TextMuted,
            Font = new Font("Segoe UI Emoji", 10f),
            Size = new Size(28, 28),
            Cursor = Cursors.Hand,
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 255, 255, 255);
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
