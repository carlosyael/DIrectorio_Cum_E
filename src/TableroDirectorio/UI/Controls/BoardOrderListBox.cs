using TableroDirectorio.Models;
using TableroDirectorio.UI.Themes;

namespace TableroDirectorio.UI.Controls;

public class BoardOrderListBox : ListBox
{
    private Point _dragStartPoint;
    private int _indexOfItemUnderMouseToDrag = -1;
    private int _targetIndex = -1;

    public event Action? OrderChanged;

    public BoardOrderListBox()
    {
        DrawMode = DrawMode.OwnerDrawFixed;
        ItemHeight = 48;
        AllowDrop = true;
        BorderStyle = BorderStyle.None;
        IntegralHeight = false;
        DoubleBuffered = true;

        BackColor = DarkTheme.InputBackground;
        ForeColor = DarkTheme.TextPrimary;
        Font = DarkTheme.CardBodyFont;

        ThemeManager.ThemeChanged += () =>
        {
            BackColor = DarkTheme.InputBackground;
            ForeColor = DarkTheme.TextPrimary;
            Invalidate();
        };
    }

    public void SetItems(IEnumerable<DirectoryItem> items)
    {
        BeginUpdate();
        Items.Clear();
        foreach (var item in items)
        {
            Items.Add(item);
        }
        EndUpdate();
    }

    public List<DirectoryItem> GetOrderedItems()
    {
        var list = new List<DirectoryItem>();
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] is DirectoryItem item)
            {
                item.SortOrder = i + 1;
                list.Add(item);
            }
        }
        return list;
    }

    public void MoveSelectedUp()
    {
        int index = SelectedIndex;
        if (index > 0)
        {
            var item = Items[index];
            Items.RemoveAt(index);
            Items.Insert(index - 1, item);
            SelectedIndex = index - 1;
            OrderChanged?.Invoke();
        }
    }

    public void MoveSelectedDown()
    {
        int index = SelectedIndex;
        if (index >= 0 && index < Items.Count - 1)
        {
            var item = Items[index];
            Items.RemoveAt(index);
            Items.Insert(index + 1, item);
            SelectedIndex = index + 1;
            OrderChanged?.Invoke();
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        _indexOfItemUnderMouseToDrag = IndexFromPoint(e.X, e.Y);
        if (_indexOfItemUnderMouseToDrag != ListBox.NoMatches)
        {
            _dragStartPoint = e.Location;
        }
        else
        {
            _indexOfItemUnderMouseToDrag = -1;
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if ((e.Button & MouseButtons.Left) == MouseButtons.Left && _indexOfItemUnderMouseToDrag != -1)
        {
            var dragRect = new Rectangle(
                _dragStartPoint.X - SystemInformation.DragSize.Width / 2,
                _dragStartPoint.Y - SystemInformation.DragSize.Height / 2,
                SystemInformation.DragSize.Width,
                SystemInformation.DragSize.Height);

            if (!dragRect.Contains(e.Location))
            {
                var itemToDrag = Items[_indexOfItemUnderMouseToDrag];
                DoDragDrop(itemToDrag, DragDropEffects.Move);
            }
        }
    }

    protected override void OnDragOver(DragEventArgs e)
    {
        base.OnDragOver(e);
        e.Effect = DragDropEffects.Move;

        var clientPoint = PointToClient(new Point(e.X, e.Y));
        var newIndex = IndexFromPoint(clientPoint);
        if (newIndex != _targetIndex)
        {
            _targetIndex = newIndex;
            Invalidate();
        }
    }

    protected override void OnDragDrop(DragEventArgs e)
    {
        base.OnDragDrop(e);
        var clientPoint = PointToClient(new Point(e.X, e.Y));
        var targetIndex = IndexFromPoint(clientPoint);

        if (targetIndex == ListBox.NoMatches)
        {
            targetIndex = Items.Count - 1;
        }

        if (e.Data?.GetData(typeof(DirectoryItem)) is DirectoryItem item &&
            _indexOfItemUnderMouseToDrag >= 0 &&
            _indexOfItemUnderMouseToDrag < Items.Count)
        {
            Items.RemoveAt(_indexOfItemUnderMouseToDrag);
            if (targetIndex >= Items.Count)
                Items.Add(item);
            else
                Items.Insert(targetIndex, item);

            SelectedIndex = targetIndex;
            _targetIndex = -1;
            Invalidate();
            OrderChanged?.Invoke();
        }
    }

    protected override void OnDragLeave(EventArgs e)
    {
        base.OnDragLeave(e);
        _targetIndex = -1;
        Invalidate();
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= Items.Count) return;

        var g = e.Graphics;
        var rect = e.Bounds;
        var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
        var item = Items[e.Index] as DirectoryItem;

        // Background
        var bg = isSelected ? DarkTheme.CardHover : (e.Index % 2 == 0 ? DarkTheme.InputBackground : DarkTheme.CardBackground);
        using (var brush = new SolidBrush(bg))
        {
            g.FillRectangle(brush, rect);
        }

        // Draw insertion indicator line if dragging over this item
        if (e.Index == _targetIndex)
        {
            using var linePen = new Pen(DarkTheme.AccentBlue, 2f);
            g.DrawLine(linePen, rect.Left, rect.Top, rect.Right, rect.Top);
        }

        if (item == null) return;

        // Drag handle & Number
        var handleText = $"☰  #{e.Index + 1}";
        var handleRect = new Rectangle(rect.X + 8, rect.Y, 55, rect.Height);
        TextRenderer.DrawText(g, handleText, DarkTheme.CardBodyFont, handleRect,
            DarkTheme.TextMuted, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

        // Icon
        var iconText = DarkTheme.GetIcon(item.IconName);
        var iconRect = new Rectangle(rect.X + 65, rect.Y, 32, rect.Height);
        TextRenderer.DrawText(g, iconText, new Font("Segoe UI Emoji", 14f), iconRect,
            DarkTheme.TextPrimary, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        // Title
        var titleRect = new Rectangle(rect.X + 105, rect.Y + 4, rect.Width - 220, 22);
        TextRenderer.DrawText(g, item.Title, DarkTheme.CardTitleFont, titleRect,
            DarkTheme.TextPrimary, TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

        // Type & Category
        var infoText = $"{item.ResourceType}  •  {item.Category}";
        var infoRect = new Rectangle(rect.X + 105, rect.Y + 24, rect.Width - 220, 20);
        TextRenderer.DrawText(g, infoText, DarkTheme.CardBodyFont, infoRect,
            DarkTheme.TextSecondary, TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

        // Color badge indicator on the right
        var colorBar = DarkTheme.GetCardColor(item.ColorHex);
        using (var barBrush = new SolidBrush(colorBar))
        {
            g.FillRectangle(barBrush, rect.Right - 8, rect.Top + 6, 4, rect.Height - 12);
        }

        e.DrawFocusRectangle();
    }
}
