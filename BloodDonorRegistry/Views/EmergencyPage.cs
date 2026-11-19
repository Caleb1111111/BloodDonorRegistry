using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Helpers;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Views;

public class EmergencyPage : UserControl
{
    private readonly ReadinessController _controller;
    private readonly ComboBox _group = new();
    private readonly Label _summary = new();
    private readonly Label _hint = new();
    private readonly DataGridView _grid = new();

    public EmergencyPage(ReadinessController controller)
    {
        _controller = controller;
        BackColor = UiTheme.Paper;
        Padding = new Padding(36, 0, 36, 30);
        BuildUi();
        ShowHint("Choose a blood group above to see who can give today.");
    }

    private void BuildUi()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 170, BackColor = UiTheme.Paper };

        top.Controls.Add(new Label
        {
            Text = "Emergency finder",
            Font = UiTheme.Title,
            AutoSize = true,
            Location = new Point(0, 30)
        });
        top.Controls.Add(new Label
        {
            Text = "Pick the blood group you need to see who can give today.",
            ForeColor = UiTheme.Muted,
            AutoSize = true,
            Location = new Point(3, 76)
        });

        _group.SetBounds(0, 116, 200, 30);
        _group.Font = UiTheme.Body;
        _group.DropDownStyle = ComboBoxStyle.DropDownList;
        _group.Items.AddRange(BloodGroups.All);
        _group.SelectedIndexChanged += (s, e) => Reload();
        top.Controls.Add(_group);

        _summary.Font = UiTheme.Bold;
        _summary.AutoSize = true;
        _summary.Location = new Point(220, 120);
        top.Controls.Add(_summary);

        _hint.Dock = DockStyle.Fill;
        _hint.TextAlign = ContentAlignment.MiddleCenter;
        _hint.ForeColor = UiTheme.Muted;

        UiTheme.StyleGrid(_grid);
        _grid.Dock = DockStyle.Fill;
        AddColumn("Name", 180);
        AddColumn("Phone", 130);
        AddColumn("District", 110);
        AddColumn("Blood group", 80);
        AddColumn("Last donation", 120);
        _grid.Columns[1].DefaultCellStyle.Font = UiTheme.Bold;   // the phone number matters most here
        GridPills.Attach(_grid, new[] { 3 });

        Controls.Add(_hint);   // Fill controls first
        Controls.Add(_grid);
        Controls.Add(top);
    }

    private void AddColumn(string header, float weight)
    {
        int i = _grid.Columns.Add(header, header);
        _grid.Columns[i].FillWeight = weight;
    }

    private void ShowHint(string text)
    {
        _hint.Text = text;
        _hint.Visible = true;
        _grid.Visible = false;
    }

    private void Reload()
    {
        _grid.Rows.Clear();
        if (_group.SelectedItem is not string group)
        {
            _summary.Text = "";
            ShowHint("Choose a blood group above to see who can give today.");
            return;
        }

        try
        {
            var donors = _controller.Emergency(group);
            foreach (var item in donors)
                _grid.Rows.Add(item.Donor.FullName, item.Donor.Phone, item.Donor.District,
                               item.Donor.BloodGroup,
                               item.LastDonation?.ToString("d MMM yyyy") ?? "Never");

            if (donors.Count == 0)
            {
                _summary.ForeColor = UiTheme.Muted;
                _summary.Text = $"No {group} donors are ready today.";
                ShowHint($"No {group} donors can give today.\nCheck Readiness to see who is closest to being ready.");
            }
            else
            {
                _summary.ForeColor = UiTheme.Ready;
                _summary.Text = $"{donors.Count} {group} {(donors.Count == 1 ? "donor is" : "donors are")} ready today";
                _hint.Visible = false;
                _grid.Visible = true;
            }
        }
        catch (Exception)
        {
            _summary.ForeColor = UiTheme.Crimson;
            _summary.Text = "Couldn't search. Is PostgreSQL running?";
        }
    }
}