using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Helpers;

namespace BloodDonorRegistry.Views;

public class ReportsPage : UserControl
{
    private readonly ReportController _controller;
    private readonly BarChart _groups = new() { Dock = DockStyle.Fill };
    private readonly BarChart _months = new() { Dock = DockStyle.Fill };
    private readonly StatCard _donorsCard = new("Total donors");
    private readonly StatCard _donationsCard = new("Total donations");
    private readonly Label _status = new();

    public ReportsPage(ReportController controller)
    {
        _controller = controller;
        BackColor = UiTheme.Paper;
        Padding = new Padding(36, 0, 36, 30);
        BuildUi();
        LoadData();
    }

    private void BuildUi()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 240, BackColor = UiTheme.Paper };

        top.Controls.Add(new Label
        {
            Text = "Reports",
            Font = UiTheme.Title,
            AutoSize = true,
            Location = new Point(0, 30)
        });

        _status.Text = DateTime.Now.ToString("'As of' d MMMM yyyy");
        _status.ForeColor = UiTheme.Muted;
        _status.AutoSize = true;
        _status.Location = new Point(3, 76);
        top.Controls.Add(_status);

        var cards = new FlowLayoutPanel
        {
            Location = new Point(0, 116),
            Size = new Size(540, 110),
            BackColor = UiTheme.Paper
        };
        cards.Controls.Add(_donorsCard);
        cards.Controls.Add(_donationsCard);
        top.Controls.Add(cards);

        var charts = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = UiTheme.Paper
        };
        charts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        charts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        charts.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        charts.Controls.Add(ChartPanel("Donors per blood group", _groups, new Padding(0, 0, 10, 0)), 0, 0);
        charts.Controls.Add(ChartPanel("Donations per month (last 6 months)", _months, new Padding(10, 0, 0, 0)), 1, 0);

        Controls.Add(charts);   // Fill first
        Controls.Add(top);
    }

    private static Panel ChartPanel(string title, BarChart chart, Padding margin)
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = margin };
        panel.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(240, 224, 221));
            e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        };

        var heading = new Label
        {
            Text = title,
            Font = UiTheme.Bold,
            Dock = DockStyle.Top,
            Height = 46,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(16, 0, 0, 0)
        };

        panel.Controls.Add(chart);     // Fill first
        panel.Controls.Add(heading);
        return panel;
    }

    private void LoadData()
    {
        try
        {
            var summary = _controller.GetSummary();
            _donorsCard.SetValue(summary.TotalDonors.ToString());
            _donationsCard.SetValue(summary.TotalDonations.ToString());
            _groups.SetData(_controller.DonorsPerBloodGroup());
            _months.SetData(_controller.DonationsPerMonth());
        }
        catch (Exception)
        {
            _status.ForeColor = UiTheme.Crimson;
            _status.Text = "Couldn't load reports. Is PostgreSQL running?";
        }
    }
}