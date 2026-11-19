using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BloodDonorRegistry.Helpers;

public class BarChart : Control
{
    private List<(string Label, int Value)> _items = new();

    public BarChart()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                 ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        BackColor = Color.White;
    }

    public void SetData(IEnumerable<(string Label, int Value)> items)
    {
        _items = items.ToList();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(BackColor);

        if (_items.Count == 0)
        {
            TextRenderer.DrawText(g, "No data yet", UiTheme.Body, ClientRectangle, UiTheme.Muted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            return;
        }

        const int pad = 12, labelW = 90, valueW = 44;
        int max = Math.Max(1, _items.Max(i => i.Value));
        int rowH = Math.Max(26, Math.Min(40, (Height - pad * 2) / _items.Count));
        int barX = pad + labelW;
        int barMaxW = Math.Max(10, Width - barX - valueW - pad);
        var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left;

        int y = pad;
        foreach (var (label, value) in _items)
        {
            TextRenderer.DrawText(g, label, UiTheme.Body, new Rectangle(pad, y, labelW, rowH), UiTheme.Ink, flags);

            int w = value == 0 ? 0 : Math.Max(4, (int)((long)barMaxW * value / max));
            if (w > 0)
            {
                using var path = RoundedRect(new Rectangle(barX, y + 5, w, rowH - 10), Math.Min(4, w / 2));
                using var brush = new SolidBrush(UiTheme.Crimson);
                g.FillPath(brush, path);
            }

            TextRenderer.DrawText(g, value.ToString(), UiTheme.Bold,
                new Rectangle(barX + w + 8, y, valueW, rowH), UiTheme.Ink, flags);
            y += rowH;
        }
    }

    private static GraphicsPath RoundedRect(Rectangle r, int radius)
    {
        int d = Math.Max(1, radius * 2);
        var p = new GraphicsPath();
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }
}