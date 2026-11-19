using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Helpers;

namespace BloodDonorRegistry.Views;

public class DashboardForm : Form
{
    private readonly Label _hint = new();

    public DashboardForm(string officerName)
    {
        Text = "LifeDrop";
        ClientSize = new Size(1100, 680);
        MinimumSize = new Size(960, 600);
        UiTheme.StyleForm(this);

        BuildContent(officerName);
        BuildSidebar(officerName);
    }

    private static string Greeting() => DateTime.Now.Hour switch
    {
        < 12 => "Good morning",
        < 17 => "Good afternoon",
        _ => "Good evening"
    };

    private void BuildSidebar(string officerName)
    {
        var sidebar = new Panel { Dock = DockStyle.Left, Width = 230, BackColor = UiTheme.Crimson };

        var nav = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = UiTheme.Crimson,
            Padding = new Padding(0, 8, 0, 0)
        };
        foreach (var item in new[] { "Dashboard", "Donors", "Donations", "Readiness",
                                     "Emergency finder", "Reports", "Settings" })
            nav.Controls.Add(NavButton(item));

        var header = new Panel { Dock = DockStyle.Top, Height = 110, BackColor = UiTheme.Crimson };
        header.Controls.Add(new DropLogo { Location = new Point(24, 28), Size = new Size(36, 44) });
        header.Controls.Add(new Label
        {
            Text = "LifeDrop",
            Font = new Font("Segoe UI Semibold", 18f),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(70, 32)
        });

        var footer = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            Text = $"Signed in as\n{officerName}",
            ForeColor = Color.FromArgb(253, 220, 216),
            Font = new Font("Segoe UI", 9f),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(20, 0, 0, 0)
        };

        sidebar.Controls.Add(nav);      // Fill first
        sidebar.Controls.Add(header);
        sidebar.Controls.Add(footer);
        Controls.Add(sidebar);
    }

    private Button NavButton(string text)
    {
        var b = new Button
        {
            Text = text,
            Width = 230,
            Height = 46,
            Margin = new Padding(0),
            Padding = new Padding(22, 0, 0, 0),
            TextAlign = ContentAlignment.MiddleLeft,
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = UiTheme.Crimson,
            Font = UiTheme.Body,
            Cursor = Cursors.Hand
        };
        b.FlatAppearance.BorderSize = 0;
        b.MouseEnter += (s, e) => b.BackColor = UiTheme.DeepRed;
        b.MouseLeave += (s, e) => b.BackColor = UiTheme.Crimson;
        b.Click += (s, e) => _hint.Text = $"{text} will appear here – coming in the next stage.";
        return b;
    }

    private void BuildContent(string officerName)
    {
        var content = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Paper };
        string firstName = officerName.Split(' ')[0];

        content.Controls.Add(new Label
        {
            Text = $"{Greeting()}, {firstName}",
            Font = UiTheme.Title,
            AutoSize = true,
            Location = new Point(36, 30)
        });
        content.Controls.Add(new Label
        {
            Text = DateTime.Now.ToString("dddd, d MMMM yyyy"),
            ForeColor = UiTheme.Muted,
            AutoSize = true,
            Location = new Point(39, 76)
        });

        var cards = new FlowLayoutPanel
        {
            Location = new Point(36, 125),
            Size = new Size(780, 120),
            BackColor = UiTheme.Paper
        };
        cards.Controls.Add(Card("Registered donors"));
        cards.Controls.Add(Card("Ready to give today"));
        cards.Controls.Add(Card("Donations this month"));
        content.Controls.Add(cards);

        _hint.Text = "Pick a section on the left to begin.";
        _hint.ForeColor = UiTheme.Muted;
        _hint.AutoSize = true;
        _hint.Location = new Point(39, 275);
        content.Controls.Add(_hint);

        Controls.Add(content);
    }

    private Panel Card(string caption)
    {
        var card = new Panel { Size = new Size(240, 104), BackColor = Color.White, Margin = new Padding(0, 0, 20, 0) };
        card.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(240, 224, 221));
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        };
        card.Controls.Add(new Label
        {
            Text = caption,
            ForeColor = UiTheme.Muted,
            AutoSize = true,
            Location = new Point(18, 16)
        });
        card.Controls.Add(new Label
        {
            Text = "–",
            Font = new Font("Segoe UI Semibold", 28f),
            ForeColor = UiTheme.Crimson,
            AutoSize = true,
            Location = new Point(16, 42)
        });
        return card;
    }
}