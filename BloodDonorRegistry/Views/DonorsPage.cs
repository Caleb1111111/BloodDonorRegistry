using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Helpers;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Views;

public class DonorsPage : UserControl
{
    private readonly DonorController _controller;
    private readonly TextBox _search = new();
    private readonly ComboBox _group = new();
    private readonly Label _count = new();
    private readonly DataGridView _grid = new();

    public DonorsPage(DonorController controller)
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
            Text = "Donors",
            Font = UiTheme.Title,
            AutoSize = true,
            Location = new Point(0, 30)
        });

        _count.ForeColor = UiTheme.Muted;
        _count.AutoSize = true;
        _count.Location = new Point(3, 76);
        top.Controls.Add(_count);

        _search.SetBounds(0, 108, 320, 30);
        _search.Font = UiTheme.Body;
        _search.PlaceholderText = "Search by name or national ID";
        top.Controls.Add(_search);

        _group.SetBounds(336, 108, 160, 30);
        _group.Font = UiTheme.Body;
        _group.DropDownStyle = ComboBoxStyle.DropDownList;
        _group.Items.Add("All blood groups");
        _group.Items.AddRange(BloodGroups.All);
        _group.SelectedIndex = 0;
        top.Controls.Add(_group);

        _search.TextChanged += (s, e) => Reload();
        _group.SelectedIndexChanged += (s, e) => Reload();

        // Action buttons, right-aligned (first added = rightmost)
        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            Width = 340,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(0, 24, 0, 0),
            BackColor = UiTheme.Paper
        };
        var add = new Button { Text = "Add donor", Width = 130, Margin = new Padding(8, 0, 0, 0) };
        UiTheme.StylePrimaryButton(add);
        add.Click += (s, e) => AddDonor();

        var edit = new Button { Text = "Edit", Width = 80, Margin = new Padding(8, 0, 0, 0) };
        UiTheme.StyleSecondaryButton(edit);
        edit.Click += (s, e) => EditSelected();

        var delete = new Button { Text = "Delete", Width = 90, Margin = new Padding(8, 0, 0, 0) };
        UiTheme.StyleSecondaryButton(delete);
        delete.Click += (s, e) => DeleteSelected();

        actions.Controls.Add(add);
        actions.Controls.Add(edit);
        actions.Controls.Add(delete);
        top.Controls.Add(actions);

        UiTheme.StyleGrid(_grid);
        _grid.Dock = DockStyle.Fill;
        _grid.MultiSelect = false;
        _grid.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditSelected(); };
        AddColumn("Name", 170);
        AddColumn("National ID", 150);
        AddColumn("Blood group", 80);
        AddColumn("Phone", 110);
        AddColumn("District", 100);
        AddColumn("Age", 50);

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
            var donors = _controller.Search(_search.Text, group);

            _grid.Rows.Clear();
            foreach (var d in donors)
            {
                int i = _grid.Rows.Add(d.FullName, d.NationalId, d.BloodGroup, d.Phone, d.District, d.Age);
                _grid.Rows[i].Tag = d;
            }
            _count.Text = donors.Count == 1 ? "1 donor" : $"{donors.Count} donors";
        }
        catch (Exception)
        {
            _count.Text = "Couldn't load donors. Is PostgreSQL running?";
        }
    }

    private void AddDonor()
    {
        using var dialog = new DonorDialog(null, _controller.Add);
        if (dialog.ShowDialog(FindForm()) == DialogResult.OK) Reload();
    }

    private void EditSelected()
    {
        if (_grid.CurrentRow?.Tag is not Donor donor)
        {
            Info("Select a donor in the table first.");
            return;
        }
        using var dialog = new DonorDialog(donor, _controller.Update);
        if (dialog.ShowDialog(FindForm()) == DialogResult.OK) Reload();
    }

    private void DeleteSelected()
    {
        if (_grid.CurrentRow?.Tag is not Donor donor)
        {
            Info("Select a donor in the table first.");
            return;
        }

        var answer = MessageBox.Show($"Delete {donor.FullName}?\nThis cannot be undone.",
            "LifeDrop", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        if (answer != DialogResult.Yes) return;

        try
        {
            var result = _controller.Delete(donor);
            if (!result.Success) Info(result.Message);
            Reload();
        }
        catch (Exception)
        {
            Info("Couldn't delete. Is PostgreSQL running?");
        }
    }

    private static void Info(string message)
        => MessageBox.Show(message, "LifeDrop", MessageBoxButtons.OK, MessageBoxIcon.Information);
}