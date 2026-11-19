using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Helpers;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Views;

public class DonationDialog : Form
{
    private readonly Func<Donation, OperationResult> _save;

    private readonly ComboBox _donor = new();
    private readonly DateTimePicker _date = new();
    private readonly TextBox _place = new();
    private readonly NumericUpDown _amount = new();
    private readonly Label _error = new();

    public DonationDialog(List<Donor> donors, Donor? preselected, Func<Donation, OperationResult> save)
    {
        _save = save;

        Text = "Record a donation";
        ClientSize = new Size(460, 490);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        UiTheme.StyleForm(this);

        BuildUi(donors, preselected);
    }

    private void BuildUi(List<Donor> donors, Donor? preselected)
    {
        Controls.Add(new Label
        {
            Text = "Record a donation",
            Font = UiTheme.Title,
            AutoSize = true,
            Location = new Point(30, 20)
        });

        _donor.DropDownStyle = ComboBoxStyle.DropDown;
        _donor.Items.AddRange(donors.ToArray());
        _donor.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        _donor.AutoCompleteSource = AutoCompleteSource.ListItems;
        if (preselected != null) _donor.SelectedItem = preselected;
        AddField("Donor (type to search)", _donor, 84);

        _date.Format = DateTimePickerFormat.Long;
        _date.MaxDate = DateTime.Today;
        _date.Value = DateTime.Today;
        AddField("Date of donation", _date, 152);

        _place.MaxLength = 100;
        AddField("Place (for example, a blood drive)", _place, 220);

        _amount.Minimum = 100;
        _amount.Maximum = 600;
        _amount.Increment = 10;
        _amount.Value = 450;
        AddField("Amount given (ml)", _amount, 288);

        _error.ForeColor = UiTheme.Crimson;
        _error.AutoSize = true;
        _error.MaximumSize = new Size(400, 0);
        _error.Location = new Point(30, 362);
        Controls.Add(_error);

        var save = new Button { Text = "Save", Location = new Point(30, 424), Width = 195 };
        UiTheme.StylePrimaryButton(save);
        save.Click += Save_Click;

        var cancel = new Button
        {
            Text = "Cancel",
            Location = new Point(235, 424),
            Width = 195,
            DialogResult = DialogResult.Cancel
        };
        UiTheme.StyleSecondaryButton(cancel);

        Controls.Add(save);
        Controls.Add(cancel);
        AcceptButton = save;
        CancelButton = cancel;
    }

    private void AddField(string caption, Control input, int y)
    {
        Controls.Add(new Label
        {
            Text = caption,
            Font = UiTheme.Bold,
            AutoSize = true,
            Location = new Point(30, y)
        });
        input.SetBounds(30, y + 24, 400, 30);
        input.Font = UiTheme.Body;
        Controls.Add(input);
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        var donor = _donor.SelectedItem as Donor;   // null if the typed text isn't a listed donor

        var donation = new Donation
        {
            DonorId = donor?.DonorId ?? 0,
            DonationDate = _date.Value.Date,
            Place = _place.Text,
            AmountMl = (int)_amount.Value
        };

        try
        {
            var result = _save(donation);
            if (!result.Success)
            {
                _error.Text = result.Message;
                return;
            }
            DialogResult = DialogResult.OK;
        }
        catch (Exception)
        {
            _error.Text = "Couldn't save. Is PostgreSQL running?";
        }
    }
}