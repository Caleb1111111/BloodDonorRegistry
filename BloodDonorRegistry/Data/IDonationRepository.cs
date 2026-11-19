using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Data;

public interface IDonationRepository
{
    // donorId null = every donation; otherwise only that donor's history
    List<DonationRecord> GetAll(int? donorId);
    void Add(Donation donation);
}