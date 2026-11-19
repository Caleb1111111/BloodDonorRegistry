using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Data;

public interface IDonorRepository
{
    // text matches name or national ID (empty = everyone); bloodGroup null = all groups
    List<Donor> Search(string text, string? bloodGroup);
}   