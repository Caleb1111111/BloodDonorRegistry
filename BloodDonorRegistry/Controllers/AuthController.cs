using BloodDonorRegistry.Data;
using BloodDonorRegistry.Helpers;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Controllers;

public class AuthController
{
    private readonly IUserRepository _users;

    public AuthController(IUserRepository users) => _users = users;

    // First run only: creates one officer account so you can sign in.
    public void EnsureDefaultOfficer()
    {
        if (_users.Count() > 0) return;
        _users.Add(new User
        {
            Username = "officer",
            PasswordHash = PasswordHasher.Hash("LifeDrop2026"),
            FullName = "Officer"
        });
    }

    // Returns the user if the login is correct, otherwise null.
    public User? Login(string username, string password)
    {
        var user = _users.FindByUsername(username);
        if (user == null || !PasswordHasher.Verify(password, user.PasswordHash)) return null;
        return user;
    }
}   