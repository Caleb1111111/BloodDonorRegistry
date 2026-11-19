using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Helpers;

namespace BloodDonorRegistry.Views;

public class DashboardForm : Form
{
    private readonly string _officerName;
    private readonly DonorController _donors;
    private readonly Panel _host = new() { Dock = DockStyle.Fill, BackColor = UiTheme.Paper };
    private readonly List<Button> _navButtons = new();
    private Button? _active;

    public DashboardForm(string officerName, DonorController donors)
    {
        _officerName = officerName;
        _donors = donors;

        Text = "LifeDrop";
        ClientSize = new Size(1100, 680);
        MinimumSize = new Size(960, 600);
        UiTheme.StyleForm(this);

        Controls.Add(_host);   // Fill first, so the sidebar docks before it
        BuildSidebar();
        Navigate("Dashboard");
    }

    private static string Greeting() => DateTime.Now.Hour switch
    {
        < 12 => "Good morning",
        < 17 => "Good afternoon",
        _ => "Good evening"
    };

    // ---------- Navigation ----------

    private void Navigate(string name)
    {
        _active = _navButtons.First(b => b.Text == name);
        foreach (var b in _navButtons)
            b.BackColor = b == _active ? UiTheme.DeepRed : UiTheme.Crimson;

        Control page = name switch
        {
            "Dashboard" => BuildHomePage(),
            "Donors" => new DonorsPage(_donors),
            _ => BuildComingSoon(name)
        };
        ShowPage(page);
    }

    private void ShowPage(Control page)
    {
        var old = _host.Controls.Cast<Control>().ToList();
        _host.Controls.Clear();
        foreach (var c in old) c.Dispose();

        page.Dock = DockStyle.Fill;
        _host.Controls.Add(page);
    }

    // ---------- Sidebar ----------

    private void BuildSidebar()
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
            Text = $"Signed in as\n{_officerName}",
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
        b.MouseEnter += (s, e) => { if (b != _active) b.BackColor = Color.FromArgb(155, 26, 26); };
        b.MouseLeave += (s, e) => { if (b != _active) b.BackColor = UiTheme.Crimson; };
        b.Click += (s, e) => Navigate(text);
        _navButtons.Add(b);
        return b;
    }

    // ---------- Pages ----------

    private Control BuildHomePage()
    {
        var page = new Panel { BackColor = UiTheme.Paper };
        string firstName = _officerName.Split(' ')[0];

        page.Controls.Add(new Label
        {
            Text = $"{Greeting()}, {firstName}",
            Font = UiTheme.Title,
            AutoSize = true,
            Location = new Point(36, 30)
        });
        page.Controls.Add(new Label
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
        page.Controls.Add(cards);

        return page;
    }

    private static Control BuildComingSoon(string name)
    {
        var page = new Panel { BackColor = UiTheme.Paper };
        page.Controls.Add(new Label
        {
            Text = name,
            Font = UiTheme.Title,
            AutoSize = true,
            Location = new Point(36, 30)
        });
        page.Controls.Add(new Label
        {
            Text = "Coming in the next stage.",
            ForeColor = UiTheme.Muted,
            AutoSize = true,
            Location = new Point(39, 76)
        });
        return page;
    }

    private static Panel Card(string caption)
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