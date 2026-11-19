using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BloodDonorRegistry.Helpers;

// A small blood-drop logo drawn in code, so we need no image files.
public class DropLogo : Control
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color DropColor { get; set; } = Color.White;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color CrossColor { get; set; } = UiTheme.Crimson;    

    public DropLogo()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint, true);
        BackColor = Color.Transparent;
        Size = new Size(48, 56);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        float w = Width, h = Height;
        float cx = w / 2f;
        float r = w * 0.42f;
        float cy = h - r - 2;

        using var drop = new SolidBrush(DropColor);
        g.FillEllipse(drop, cx - r, cy - r, 2 * r, 2 * r);
        g.FillPolygon(drop, new[]
        {
            new PointF(cx, 2),
            new PointF(cx + r * 0.9f, cy - r * 0.45f),
            new PointF(cx - r * 0.9f, cy - r * 0.45f)
        });

        using var cross = new SolidBrush(CrossColor);
        float arm = r * 0.9f, thick = r * 0.3f;
        g.FillRectangle(cross, cx - arm / 2, cy - thick / 2, arm, thick);
        g.FillRectangle(cross, cx - thick / 2, cy - arm / 2, thick, arm);
    }
}