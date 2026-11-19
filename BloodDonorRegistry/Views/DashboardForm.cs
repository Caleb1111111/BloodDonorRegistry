using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Helpers;

namespace BloodDonorRegistry.Views;

public class DashboardForm : Form
{
    private readonly string _officerName;
    private readonly AppServices _services;
    private readonly Panel _host = new() { Dock = DockStyle.Fill, BackColor = UiTheme.Paper };
    private readonly List<Button> _navButtons = new();
    private Button? _active;

    public DashboardForm(string officerName, AppServices services)
    {
        _officerName = officerName;
        _services = services;

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
            "Donors" => new DonorsPage(_services.Donors),
            "Donations" => new DonationsPage(_services.Donations, _services.Donors),
            "Readiness" => new ReadinessPage(_services.Readiness),
            "Reports" => new ReportsPage(_services.Reports),
            "Emergency finder" => new EmergencyPage(_services.Readiness),
            "Settings" => new SettingsPage(_services.Settings),
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

        var footer = new Panel { Dock = DockStyle.Bottom, Height = 110, BackColor = UiTheme.Crimson };
        footer.Controls.Add(new Label
        {
            Text = $"Signed in as\n{_officerName}",
            ForeColor = Color.FromArgb(253, 220, 216),
            Font = new Font("Segoe UI", 9f),
            AutoSize = true,
            Location = new Point(20, 14)
        });

        var signOut = new Button
        {
            Text = "Sign out",
            Location = new Point(20, 62),
            Size = new Size(190, 34),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = UiTheme.Crimson,
            Font = UiTheme.Body,
            Cursor = Cursors.Hand
        };
        signOut.FlatAppearance.BorderSize = 1;
        signOut.FlatAppearance.BorderColor = Color.FromArgb(253, 220, 216);
        signOut.MouseEnter += (s, e) => signOut.BackColor = UiTheme.DeepRed;
        signOut.MouseLeave += (s, e) => signOut.BackColor = UiTheme.Crimson;
        signOut.Click += (s, e) => DialogResult = DialogResult.Retry;   // tells Program to show the login again
        footer.Controls.Add(signOut);

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

        var donorsCard = new StatCard("Registered donors");
        var readyCard = new StatCard("Ready to give today");
        var monthCard = new StatCard("Donations this month");

        var cards = new FlowLayoutPanel
        {
            Location = new Point(36, 125),
            Size = new Size(820, 120),
            BackColor = UiTheme.Paper
        };
        cards.Controls.Add(donorsCard);
        cards.Controls.Add(readyCard);
        cards.Controls.Add(monthCard);
        page.Controls.Add(cards);

        try
        {
            var summary = _services.Reports.GetSummary();
            donorsCard.SetValue(summary.TotalDonors.ToString());
            readyCard.SetValue(summary.ReadyToday.ToString());
            monthCard.SetValue(summary.DonationsThisMonth.ToString());
        }
        catch (Exception)
        {
            // Leave the cards showing "–" if the database can't be reached
        }

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

    
}   