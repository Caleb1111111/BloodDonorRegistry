using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BloodDonorRegistry.Helpers;

public static class UiTheme
{
    // Colours
    public static readonly Color Crimson = Color.FromArgb(183, 28, 28);
    public static readonly Color DeepRed = Color.FromArgb(127, 29, 29);
    public static readonly Color Rose = Color.FromArgb(253, 236, 234);
    public static readonly Color Paper = Color.FromArgb(255, 250, 248);
    public static readonly Color Ink = Color.FromArgb(46, 36, 36);
    public static readonly Color Muted = Color.FromArgb(125, 108, 108);
    public static readonly Color Ready = Color.FromArgb(46, 125, 50);
    public static readonly Color Waiting = Color.FromArgb(214, 110, 20);
    public static readonly Color Line = Color.FromArgb(214, 196, 192);

    // Fonts: change FontFamily here and the whole app follows
    public const string FontFamily = "Segoe UI";
    public static readonly Font Body = new(FontFamily, 10f);
    public static readonly Font Bold = new(FontFamily, 10f, FontStyle.Bold);
    public static readonly Font Title = new(FontFamily, 20f, FontStyle.Bold);

    public static Font Make(float size, FontStyle style = FontStyle.Regular)
        => new(FontFamily, size, style);

    public static void StyleForm(Form form)
    {
        form.BackColor = Paper;
        form.ForeColor = Ink;
        form.Font = Body;
        form.StartPosition = FormStartPosition.CenterScreen;
    }

    // Main action: filled crimson
    public static void StylePrimaryButton(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.BackColor = Crimson;
        b.ForeColor = Color.White;
        b.Font = Bold;
        b.Cursor = Cursors.Hand;
        b.Height = 38;
        b.MouseEnter += (s, e) => b.BackColor = DeepRed;
        b.MouseLeave += (s, e) => b.BackColor = Crimson;
    }

    // Everyday action (Edit, Cancel): neutral outline
    public static void StyleSecondaryButton(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 1;
        b.FlatAppearance.BorderColor = Line;
        b.BackColor = Color.White;
        b.ForeColor = Ink;
        b.Font = Bold;
        b.Cursor = Cursors.Hand;
        b.Height = 38;
        b.MouseEnter += (s, e) => b.BackColor = Color.FromArgb(246, 240, 238);
        b.MouseLeave += (s, e) => b.BackColor = Color.White;
    }

    // Destructive action (Delete): neutral until you hover, then it warns in crimson
    public static void StyleDangerButton(Button b)
    {
        StyleSecondaryButton(b);
        b.MouseEnter += (s, e) =>
        {
            b.BackColor = Crimson;
            b.ForeColor = Color.White;
            b.FlatAppearance.BorderColor = Crimson;
        };
        b.MouseLeave += (s, e) =>
        {
            b.BackColor = Color.White;
            b.ForeColor = Ink;
            b.FlatAppearance.BorderColor = Line;
        };
    }

    public static void StyleGrid(DataGridView g)
    {
        g.BorderStyle = BorderStyle.None;
        g.BackgroundColor = Paper;
        g.GridColor = Color.FromArgb(240, 224, 221);
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.RowHeadersVisible = false;
        g.AllowUserToAddRows = false;
        g.AllowUserToResizeRows = false;
        g.ReadOnly = true;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.ColumnHeadersHeight = 38;
        var header = g.ColumnHeadersDefaultCellStyle;
        header.BackColor = Rose;
        header.ForeColor = DeepRed;
        header.Font = Bold;
        header.SelectionBackColor = Rose;      // stops the default Windows blue
        header.SelectionForeColor = DeepRed;
        header.Padding = new Padding(8, 0, 8, 0);

        g.RowTemplate.Height = 34;
        g.DefaultCellStyle.BackColor = Color.White;
        g.DefaultCellStyle.ForeColor = Ink;
        g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(250, 217, 213);
        g.DefaultCellStyle.SelectionForeColor = Ink;
        g.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
        g.AlternatingRowsDefaultCellStyle.BackColor = Paper;
    }

    // Numbers read better right-aligned
    public static void AlignRight(DataGridView g, params int[] columns)
    {
        foreach (int c in columns)
        {
            g.Columns[c].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            g.Columns[c].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }

    public static GraphicsPath RoundedRect(Rectangle r, int radius)
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