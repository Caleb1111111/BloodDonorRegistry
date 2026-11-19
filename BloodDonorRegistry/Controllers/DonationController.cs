using BloodDonorRegistry.Data;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Controllers;

public class DonationController
{
    private const int MinMl = 100;
    private const int MaxMl = 600;
    private readonly IDonationRepository _donations;

    public DonationController(IDonationRepository donations) => _donations = donations;

    public List<DonationRecord> History(int? donorId) => _donations.GetAll(donorId);

    public OperationResult Record(Donation donation)
    {
        donation.Place = donation.Place.Trim();
        donation.DonationDate = donation.DonationDate.Date;

        if (donation.DonorId <= 0) return OperationResult.Fail("Please choose a donor from the list.");
        if (donation.DonationDate > DateTime.Today) return OperationResult.Fail("The donation date cannot be in the future.");
        if (donation.Place.Length == 0) return OperationResult.Fail("Please enter where the donation took place.");
        if (donation.AmountMl < MinMl || donation.AmountMl > MaxMl)
            return OperationResult.Fail($"The amount must be between {MinMl} and {MaxMl} ml.");

        _donations.Add(donation);
        return OperationResult.Ok("Donation recorded.");
    }
}