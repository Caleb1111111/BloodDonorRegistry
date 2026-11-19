using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Helpers;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Views;

public class ReadinessPage : UserControl
{
    private readonly ReadinessController _controller;
    private readonly TextBox _search = new();
    private readonly ComboBox _group = new();
    private readonly ComboBox _status = new();
    private readonly Label _count = new();
    private readonly DataGridView _grid = new();

    public ReadinessPage(ReadinessController controller)
    {
        _controller = controller;
        BackColor = UiTheme.Paper;
        Padding = new Padding(36, 0, 36, 30);
        BuildUi();
        Reload();
    }

    private void BuildUi()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 150, BackColor = UiTheme.Paper };

        top.Controls.Add(new Label
        {
            Text = "Readiness",
            Font = UiTheme.Title,
            AutoSize = true,
            Location = new Point(0, 30)
        });

        _count.ForeColor = UiTheme.Muted;
        _count.AutoSize = true;
        _count.Location = new Point(3, 76);
        top.Controls.Add(_count);

        _search.SetBounds(0, 108, 280, 30);
        _search.Font = UiTheme.Body;
        _search.PlaceholderText = "Search by name or national ID";
        top.Controls.Add(_search);

        _group.SetBounds(296, 108, 150, 30);
        _group.Font = UiTheme.Body;
        _group.DropDownStyle = ComboBoxStyle.DropDownList;
        _group.Items.Add("All blood groups");
        _group.Items.AddRange(BloodGroups.All);
        _group.SelectedIndex = 0;
        top.Controls.Add(_group);

        _status.SetBounds(462, 108, 150, 30);
        _status.Font = UiTheme.Body;
        _status.DropDownStyle = ComboBoxStyle.DropDownList;
        _status.Items.AddRange(new object[] { "All statuses", "Ready", "Waiting" });
        _status.SelectedIndex = 0;
        top.Controls.Add(_status);

        _search.TextChanged += (s, e) => Reload();
        _group.SelectedIndexChanged += (s, e) => Reload();
        _status.SelectedIndexChanged += (s, e) => Reload();

        UiTheme.StyleGrid(_grid);
        _grid.Dock = DockStyle.Fill;
        AddColumn("Name", 170);
        AddColumn("Blood group", 80);
        AddColumn("Last donation", 110);
        AddColumn("Status", 80);
        AddColumn("Days left", 70);
        AddColumn("Ready from", 110);

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
        try
        {
            string? group = _group.SelectedIndex > 0 ? _group.SelectedItem!.ToString() : null;
            string status = _status.SelectedIndex switch { 1 => "Ready", 2 => "Waiting", _ => "All" };
            var items = _controller.Check(_search.Text, group, status);

            _grid.Rows.Clear();
            foreach (var item in items)
            {
                int i = _grid.Rows.Add(
                    item.Donor.FullName,
                    item.Donor.BloodGroup,
                    item.LastDonation?.ToString("d MMM yyyy") ?? "Never",
                    item.StatusText,
                    item.IsReady ? "–" : item.DaysLeft.ToString(),
                    item.ReadyOn?.ToString("d MMM yyyy") ?? "–");

                var cell = _grid.Rows[i].Cells[3];
                cell.Style.ForeColor = item.IsReady ? UiTheme.Ready : UiTheme.Waiting;
                cell.Style.Font = UiTheme.Bold;
            }

            int ready = items.Count(x => x.IsReady);
            _count.Text = $"{items.Count} shown · {ready} ready · {items.Count - ready} waiting";
        }
        catch (Exception)
        {
            _count.Text = "Couldn't load readiness. Is PostgreSQL running?";
        }
    }
}