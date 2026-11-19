using BloodDonorRegistry.Data;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Controllers;

public class DonorController
{
    private readonly IDonorRepository _donors;

    public DonorController(IDonorRepository donors) => _donors = donors;

    public List<Donor> Search(string text, string? bloodGroup)
        => _donors.Search(text.Trim(), bloodGroup);
}