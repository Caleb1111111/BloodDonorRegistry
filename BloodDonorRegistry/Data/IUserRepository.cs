using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Data;

public interface IUserRepository
{
    User? FindByUsername(string username);
    int Count();
    void Add(User user);
}