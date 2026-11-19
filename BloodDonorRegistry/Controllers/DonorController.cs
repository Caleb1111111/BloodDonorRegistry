using System.Text.RegularExpressions;
using BloodDonorRegistry.Data;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Controllers;

public class DonorController
{
    private const int MinimumAge = 18;
    private readonly IDonorRepository _donors;

    public DonorController(IDonorRepository donors) => _donors = donors;

    public List<Donor> Search(string text, string? bloodGroup)
        => _donors.Search(text.Trim(), bloodGroup);

    public OperationResult Add(Donor donor)
    {
        Clean(donor);
        string? error = Validate(donor);
        if (error != null) return OperationResult.Fail(error);

        _donors.Add(donor);
        return OperationResult.Ok("Donor registered.");
    }

    public OperationResult Update(Donor donor)
    {
        Clean(donor);
        string? error = Validate(donor);
        if (error != null) return OperationResult.Fail(error);

        _donors.Update(donor);
        return OperationResult.Ok("Donor updated.");
    }

    public OperationResult Delete(Donor donor)
    {
        if (_donors.HasDonations(donor.DonorId))
            return OperationResult.Fail(
                $"{donor.FullName} has donations on record, so this donor cannot be deleted.");

        _donors.Delete(donor.DonorId);
        return OperationResult.Ok("Donor deleted.");
    }

    private static void Clean(Donor d)
    {
        d.FullName = d.FullName.Trim();
        d.NationalId = d.NationalId.Trim();
        d.Phone = d.Phone.Trim();
        d.District = d.District.Trim();
        d.DateOfBirth = d.DateOfBirth.Date;
    }

    // Returns the first problem found, or null when everything is fine.
    private string? Validate(Donor d)
    {
        if (d.FullName.Length < 3) return "Please enter the donor's full name.";
        if (!Regex.IsMatch(d.NationalId, @"^\d{16}$")) return "National ID must be exactly 16 digits.";
        if (!Regex.IsMatch(d.Phone, @"^\+?\d{9,13}$")) return "Phone number must have 9 to 13 digits (a leading + is allowed).";
        if (d.DateOfBirth > DateTime.Today) return "Date of birth cannot be in the future.";
        if (d.Age < MinimumAge) return $"A donor must be at least {MinimumAge} years old.";
        if (d.District.Length == 0) return "Please enter the district.";
        if (!BloodGroups.All.Contains(d.BloodGroup)) return "Please choose a blood group.";
        if (_donors.NationalIdExists(d.NationalId, d.DonorId)) return "A donor with this national ID is already registered.";
        return null;
    }
}