using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Data;

public interface IReadinessRepository
{
    // Each donor with the date of their latest donation (null = never donated)
    List<DonorReadiness> GetDonorsWithLastDonation(string text, string? bloodGroup);
}