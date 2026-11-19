using System.Drawing;
using System.Windows.Forms;

namespace BloodDonorRegistry.Helpers;

public class StatCard : Panel
{
    private readonly Label _value = new();

    public StatCard(string caption)
    {
        Size = new Size(240, 104);
        BackColor = Color.White;
        Margin = new Padding(0, 0, 20, 0);

        Controls.Add(new Label
        {
            Text = caption,
            ForeColor = UiTheme.Muted,
            AutoSize = true,
            Location = new Point(18, 16)
        });

        _value.Text = "–";
        _value.Font = new Font(UiTheme.Title.FontFamily, 28f, FontStyle.Bold);
        _value.ForeColor = UiTheme.Crimson;
        _value.AutoSize = true;
        _value.Location = new Point(16, 42);
        Controls.Add(_value);
    }

    public void SetValue(string value) => _value.Text = value;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Color.FromArgb(240, 224, 221));
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }
}