using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Helpers;

namespace BloodDonorRegistry.Views;

public class LoginForm : Form
{
    private readonly TextBox _username = new();
    private readonly TextBox _password = new();
    private readonly Label _error = new();

    public string LoggedInName { get; private set; } = "";

    public LoginForm()
    {
        Text = "LifeDrop – Sign in";
        ClientSize = new Size(820, 480);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        UiTheme.StyleForm(this);
        BuildUi();
    }

    private void BuildUi()
    {
        // Left: crimson brand panel
        var left = new Panel { Dock = DockStyle.Left, Width = 340, BackColor = UiTheme.Crimson };
        var logo = new DropLogo { Location = new Point(40, 70), Size = new Size(54, 64) };
        var brand = new Label
        {
            Text = "LifeDrop",
            Font = new Font("Segoe UI Semibold", 26f),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(34, 150)
        };
        var tagline = new Label
        {
            Text = "Know who can give,\nthe moment it matters.",
            Font = new Font("Segoe UI", 12f),
            ForeColor = Color.FromArgb(253, 220, 216),
            AutoSize = true,
            Location = new Point(38, 210)
        };
        left.Controls.AddRange(new Control[] { logo, brand, tagline });

        // Right: the form
        var right = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Paper };

        var title = new Label
        {
            Text = "Welcome back",
            Font = UiTheme.Title,
            ForeColor = UiTheme.Ink,
            AutoSize = true,
            Location = new Point(60, 90)
        };
        var sub = new Label
        {
            Text = "Sign in to the donor registry",
            ForeColor = UiTheme.Muted,
            AutoSize = true,
            Location = new Point(63, 138)
        };

        var userLabel = new Label { Text = "Username", AutoSize = true, Location = new Point(62, 190), Font = UiTheme.Bold };
        _username.SetBounds(62, 214, 360, 30);
        _username.Font = new Font("Segoe UI", 11f);

        var passLabel = new Label { Text = "Password", AutoSize = true, Location = new Point(62, 262), Font = UiTheme.Bold };
        _password.SetBounds(62, 286, 360, 30);
        _password.Font = new Font("Segoe UI", 11f);
        _password.UseSystemPasswordChar = true;

        _error.ForeColor = UiTheme.Crimson;
        _error.AutoSize = true;
        _error.Location = new Point(62, 328);

        var signIn = new Button { Text = "Sign in", Location = new Point(62, 362), Width = 360 };
        UiTheme.StylePrimaryButton(signIn);
        signIn.Click += SignIn_Click;
        AcceptButton = signIn;   // pressing Enter signs in

        right.Controls.AddRange(new Control[]
            { title, sub, userLabel, _username, passLabel, _password, _error, signIn });

        // Fill goes in first so the left panel docks before it
        Controls.Add(right);
        Controls.Add(left);
    }

    private void SignIn_Click(object? sender, EventArgs e)
    {
        string user = _username.Text.Trim();
        if (user.Length == 0 || _password.Text.Length == 0)
        {
            _error.Text = "Please enter your username and password.";
            return;
        }

        // TEMPORARY: accepts any login. Stage 6 replaces this with AuthController.
        LoggedInName = user;
        DialogResult = DialogResult.OK;
    }
}