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
    private readonly DataGridView _grid = new();

    public EmergencyPage(ReadinessController controller)
    {
        _controller = controller;
        BackColor = UiTheme.Paper;
        Padding = new Padding(36, 0, 36, 30);
        BuildUi();
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

        UiTheme.StyleGrid(_grid);
        _grid.Dock = DockStyle.Fill;
        AddColumn("Name", 180);
        AddColumn("Phone", 130);
        AddColumn("District", 110);
        AddColumn("Blood group", 80);
        AddColumn("Last donation", 120);

        Controls.Add(_grid);   // Fill first
        Controls.Add(top);
    }

    private void AddColumn(string header, float weight)
    {
        int i = _grid.Columns.Add(header, header);
        _grid.Columns[i].FillWeight = weight;
    }

    private void Reload()
    {
        _grid.Rows.Clear();
        if (_group.SelectedItem is not string group) return;

        try
        {
            var donors = _controller.Emergency(group);
            foreach (var item in donors)
                _grid.Rows.Add(item.Donor.FullName, item.Donor.Phone, item.Donor.District,
                               item.Donor.BloodGroup,
                               item.LastDonation?.ToString("d MMM yyyy") ?? "Never");

            if (donors.Count == 0)
            {
                _summary.ForeColor = UiTheme.Waiting;
                _summary.Text = $"No {group} donors are ready today.";
            }
            else
            {
                _summary.ForeColor = UiTheme.Ready;
                _summary.Text = $"{donors.Count} {group} {(donors.Count == 1 ? "donor is" : "donors are")} ready today";
            }
        }
        catch (Exception)
        {
            _summary.ForeColor = UiTheme.Crimson;
            _summary.Text = "Couldn't search. Is PostgreSQL running?";
        }
    }
}