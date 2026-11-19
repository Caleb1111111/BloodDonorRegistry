using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Helpers;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Views;

public class DonationsPage : UserControl
{
    private readonly DonationController _controller;
    private readonly DonorController _donorController;
    private readonly ComboBox _filter = new();
    private readonly Label _count = new();
    private readonly DataGridView _grid = new();
    private List<Donor> _donors = new();

    public DonationsPage(DonationController controller, DonorController donorController)
    {
        _controller = controller;
        _donorController = donorController;

        BackColor = UiTheme.Paper;
        Padding = new Padding(36, 0, 36, 30);

        LoadDonors();
        BuildUi();
        Reload();
    }

    private void LoadDonors()
    {
        try { _donors = _donorController.Search("", null); }
        catch (Exception) { _donors = new List<Donor>(); }
    }

    private void BuildUi()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 150, BackColor = UiTheme.Paper };

        top.Controls.Add(new Label
        {
            Text = "Donations",
            Font = UiTheme.Title,
            AutoSize = true,
            Location = new Point(0, 30)
        });

        _count.ForeColor = UiTheme.Muted;
        _count.AutoSize = true;
        _count.Location = new Point(3, 76);
        top.Controls.Add(_count);

        top.Controls.Add(new Label
        {
            Text = "Show donations for",
            ForeColor = UiTheme.Muted,
            AutoSize = true,
            Location = new Point(0, 113)
        });
        _filter.SetBounds(150, 108, 360, 30);
        _filter.Font = UiTheme.Body;
        _filter.DropDownStyle = ComboBoxStyle.DropDownList;
        _filter.Items.Add("All donors");
        _filter.Items.AddRange(_donors.ToArray());
        _filter.SelectedIndex = 0;
        _filter.SelectedIndexChanged += (s, e) => Reload();
        top.Controls.Add(_filter);

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            Width = 220,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(0, 24, 0, 0),
            BackColor = UiTheme.Paper
        };
        var record = new Button { Text = "Record donation", Width = 190 };
        UiTheme.StylePrimaryButton(record);
        record.Click += (s, e) => RecordDonation();
        actions.Controls.Add(record);
        top.Controls.Add(actions);

        UiTheme.StyleGrid(_grid);
        _grid.Dock = DockStyle.Fill;
        AddColumn("Date", 100);
        AddColumn("Donor", 180);
        AddColumn("Blood group", 80);
        AddColumn("Place", 180);
        AddColumn("Amount (ml)", 90);

        UiTheme.AlignRight(_grid, 4);
        GridPills.Attach(_grid, new[] { 2 });

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
            var donor = _filter.SelectedItem as Donor;
            var records = _controller.History(donor?.DonorId);

            _grid.Rows.Clear();
            foreach (var d in records)
                _grid.Rows.Add(d.DonationDate.ToString("d MMM yyyy"), d.DonorName, d.BloodGroup, d.Place, d.AmountMl);

            string noun = records.Count == 1 ? "donation" : "donations";
            _count.Text = donor == null
                ? $"{records.Count} {noun}"
                : $"{records.Count} {noun} by {donor.FullName}";
        }
        catch (Exception)
        {
            _count.Text = "Couldn't load donations. Is PostgreSQL running?";
        }
    }

    private void RecordDonation()
    {
        if (_donors.Count == 0)
        {
            MessageBox.Show("Register a donor first, then record their donation.",
                "LifeDrop", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new DonationDialog(_donors, _filter.SelectedItem as Donor, _controller.Record);
        if (dialog.ShowDialog(FindForm()) == DialogResult.OK) Reload();
    }
}