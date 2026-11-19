using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Helpers;

namespace BloodDonorRegistry.Views;

public class SettingsPage : UserControl
{
    private readonly SettingsController _controller;
    private readonly NumericUpDown _months = new();
    private readonly Label _message = new();

    public SettingsPage(SettingsController controller)
    {
        _controller = controller;
        BackColor = UiTheme.Paper;
        BuildUi();
        LoadCurrent();
    }

    private void BuildUi()
    {
        Controls.Add(new Label
        {
            Text = "Settings",
            Font = UiTheme.Title,
            AutoSize = true,
            Location = new Point(36, 30)
        });
        Controls.Add(new Label
        {
            Text = "Rules used across LifeDrop",
            ForeColor = UiTheme.Muted,
            AutoSize = true,
            Location = new Point(39, 76)
        });

        var card = new Panel { Location = new Point(36, 125), Size = new Size(580, 200), BackColor = Color.White };
        card.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(240, 224, 221));
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        };

        card.Controls.Add(new Label
        {
            Text = "Waiting period between donations",
            Font = UiTheme.Bold,
            AutoSize = true,
            Location = new Point(24, 22)
        });
        card.Controls.Add(new Label
        {
            Text = "How long a donor must wait after giving blood before they show as Ready.",
            ForeColor = UiTheme.Muted,
            AutoSize = true,
            MaximumSize = new Size(520, 0),
            Location = new Point(24, 52)
        });

        _months.SetBounds(24, 110, 90, 30);
        _months.Font = UiTheme.Body;
        _months.Minimum = 1;
        _months.Maximum = 12;
        card.Controls.Add(_months);

        card.Controls.Add(new Label { Text = "months", AutoSize = true, Location = new Point(124, 114) });

        var save = new Button { Text = "Save", Location = new Point(24, 150), Width = 120 };
        UiTheme.StylePrimaryButton(save);
        save.Click += Save_Click;
        card.Controls.Add(save);

        _message.AutoSize = true;
        _message.Location = new Point(160, 158);
        card.Controls.Add(_message);

        Controls.Add(card);
    }

    private void LoadCurrent()
    {
        try
        {
            int current = _controller.GetWaitingMonths();
            _months.Value = Math.Clamp(current, (int)_months.Minimum, (int)_months.Maximum);
        }
        catch (Exception)
        {
            _message.ForeColor = UiTheme.Crimson;
            _message.Text = "Couldn't load. Is PostgreSQL running?";
        }
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        try
        {
            var result = _controller.SaveWaitingMonths((int)_months.Value);
            _message.ForeColor = result.Success ? UiTheme.Ready : UiTheme.Crimson;
            _message.Text = result.Message;
        }
        catch (Exception)
        {
            _message.ForeColor = UiTheme.Crimson;
            _message.Text = "Couldn't save. Is PostgreSQL running?";
        }
    }
}