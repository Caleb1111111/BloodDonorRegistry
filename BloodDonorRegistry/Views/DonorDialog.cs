using System.Drawing;
using System.Windows.Forms;
using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Helpers;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Views;

public class DonorDialog : Form
{
    private readonly Donor? _existing;
    private readonly Func<Donor, OperationResult> _save;

    private readonly TextBox _name = new();
    private readonly TextBox _nationalId = new();
    private readonly TextBox _phone = new();
    private readonly DateTimePicker _dob = new();
    private readonly TextBox _district = new();
    private readonly ComboBox _group = new();
    private readonly Label _error = new();

    public DonorDialog(Donor? existing, Func<Donor, OperationResult> save)
    {
        _existing = existing;
        _save = save;

        Text = existing == null ? "Register a donor" : "Edit donor";
        ClientSize = new Size(460, 590);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        UiTheme.StyleForm(this);

        BuildUi();
        if (existing != null) FillFrom(existing);
    }

    private void BuildUi()
    {
        Controls.Add(new Label
        {
            Text = _existing == null ? "Register a donor" : "Edit donor",
            Font = UiTheme.Title,
            AutoSize = true,
            Location = new Point(30, 20)
        });

        AddField("Full name", _name, 84);

        _nationalId.MaxLength = 16;
        AddField("National ID (16 digits)", _nationalId, 152);

        _phone.MaxLength = 14;
        AddField("Phone number", _phone, 220);

        _dob.Format = DateTimePickerFormat.Long;
        _dob.MaxDate = DateTime.Today;
        _dob.Value = DateTime.Today.AddYears(-25);
        AddField("Date of birth", _dob, 288);

        AddField("District", _district, 356);

        _group.DropDownStyle = ComboBoxStyle.DropDownList;
        _group.Items.AddRange(BloodGroups.All);
        AddField("Blood group", _group, 424);

        _error.ForeColor = UiTheme.Crimson;
        _error.AutoSize = true;
        _error.MaximumSize = new Size(400, 0);
        _error.Location = new Point(30, 492);
        Controls.Add(_error);

        var save = new Button { Text = "Save", Location = new Point(30, 532), Width = 195 };
        UiTheme.StylePrimaryButton(save);
        save.Click += Save_Click;

        var cancel = new Button
        {
            Text = "Cancel",
            Location = new Point(235, 532),
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

    private void FillFrom(Donor d)
    {
        _name.Text = d.FullName;
        _nationalId.Text = d.NationalId;
        _phone.Text = d.Phone;
        _dob.Value = d.DateOfBirth;
        _district.Text = d.District;
        _group.SelectedItem = d.BloodGroup;
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        var donor = new Donor
        {
            DonorId = _existing?.DonorId ?? 0,
            FullName = _name.Text,
            NationalId = _nationalId.Text,
            Phone = _phone.Text,
            DateOfBirth = _dob.Value.Date,
            District = _district.Text,
            BloodGroup = _group.SelectedItem?.ToString() ?? ""
        };

        try
        {
            var result = _save(donor);
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