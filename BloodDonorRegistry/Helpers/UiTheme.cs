using System.Drawing;
using System.Windows.Forms;

namespace BloodDonorRegistry.Helpers;

public static class UiTheme
{
    // Colours: crimson from the LifeDrop logo, warm paper background
    public static readonly Color Crimson = Color.FromArgb(183, 28, 28);
    public static readonly Color DeepRed = Color.FromArgb(127, 29, 29);
    public static readonly Color Rose = Color.FromArgb(253, 236, 234);
    public static readonly Color Paper = Color.FromArgb(255, 250, 248);
    public static readonly Color Ink = Color.FromArgb(46, 36, 36);
    public static readonly Color Muted = Color.FromArgb(125, 108, 108);
    public static readonly Color Ready = Color.FromArgb(46, 125, 50);
    public static readonly Color Waiting = Color.FromArgb(214, 110, 20);

    // Fonts
    public static readonly Font Body = new("Segoe UI", 10f);
    public static readonly Font Bold = new("Segoe UI Semibold", 10f);
    public static readonly Font Title = new("Segoe UI Semibold", 20f);

    public static void StyleForm(Form form)
    {
        form.BackColor = Paper;
        form.ForeColor = Ink;
        form.Font = Body;
        form.StartPosition = FormStartPosition.CenterScreen;
    }

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

    public static void StyleSecondaryButton(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 1;
        b.FlatAppearance.BorderColor = Crimson;
        b.BackColor = Color.White;
        b.ForeColor = Crimson;
        b.Font = Bold;
        b.Cursor = Cursors.Hand;
        b.Height = 38;
        b.MouseEnter += (s, e) => b.BackColor = Rose;
        b.MouseLeave += (s, e) => b.BackColor = Color.White;
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
        g.ColumnHeadersDefaultCellStyle.BackColor = Rose;
        g.ColumnHeadersDefaultCellStyle.ForeColor = DeepRed;
        g.ColumnHeadersDefaultCellStyle.Font = Bold;

        g.RowTemplate.Height = 34;
        g.DefaultCellStyle.BackColor = Color.White;
        g.DefaultCellStyle.ForeColor = Ink;
        g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(250, 217, 213);
        g.DefaultCellStyle.SelectionForeColor = Ink;
        g.AlternatingRowsDefaultCellStyle.BackColor = Paper;
    }
}