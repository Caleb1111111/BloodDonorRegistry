using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BloodDonorRegistry.Helpers;

// Draws blood groups and Ready/Waiting statuses as small rounded "pills".
public static class GridPills
{
    public static void Attach(DataGridView grid, int[] bloodGroupColumns, int statusColumn = -1)
    {
        grid.CellPainting += (s, e) =>
        {
            if (e.RowIndex < 0 || e.Value is not string text || text.Length == 0) return;

            if (e.ColumnIndex == statusColumn)
            {
                bool ready = text == "Ready";
                Paint(e, text,
                    ready ? Color.FromArgb(225, 243, 226) : Color.FromArgb(255, 236, 212),
                    ready ? Color.FromArgb(27, 94, 32) : Color.FromArgb(140, 70, 0));
            }
            else if (Array.IndexOf(bloodGroupColumns, e.ColumnIndex) >= 0)
            {
                Paint(e, text, UiTheme.Rose, UiTheme.DeepRed);
            }
        };
    }

    private static void Paint(DataGridViewCellPaintingEventArgs e, string text, Color back, Color fore)
    {
        e.PaintBackground(e.CellBounds, true);
        var g = e.Graphics!;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var textSize = TextRenderer.MeasureText(g, text, UiTheme.Bold);
        int h = Math.Min(e.CellBounds.Height - 12, textSize.Height + 4);
        var rect = new Rectangle(e.CellBounds.X + 8,
                                 e.CellBounds.Y + (e.CellBounds.Height - h) / 2,
                                 textSize.Width + 16, h);

        using var path = UiTheme.RoundedRect(rect, h / 2);
        using var brush = new SolidBrush(back);
        g.FillPath(brush, path);
        TextRenderer.DrawText(g, text, UiTheme.Bold, rect, fore,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        e.Handled = true;
    }
}