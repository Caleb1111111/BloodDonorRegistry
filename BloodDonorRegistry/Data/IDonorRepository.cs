using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Data;

public interface IDonorRepository
{
    List<Donor> Search(string text, string? bloodGroup);
    void Add(Donor donor);
    void Update(Donor donor);
    void Delete(int donorId);
    bool NationalIdExists(string nationalId, int excludeDonorId);
    bool HasDonations(int donorId);
}