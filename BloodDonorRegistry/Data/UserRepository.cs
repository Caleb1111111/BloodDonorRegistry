using BloodDonorRegistry.Models;
using Npgsql;

namespace BloodDonorRegistry.Data;

public class UserRepository : IUserRepository
{
    public User? FindByUsername(string username)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT user_id, username, password_hash, full_name FROM users WHERE username = @u", conn);
        cmd.Parameters.AddWithValue("u", username);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return new User
        {
            UserId = r.GetInt32(0),
            Username = r.GetString(1),
            PasswordHash = r.GetString(2),
            FullName = r.GetString(3)
        };
    }

    public int Count()
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM users", conn);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void Add(User user)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "INSERT INTO users (username, password_hash, full_name) VALUES (@u, @p, @n)", conn);
        cmd.Parameters.AddWithValue("u", user.Username);
        cmd.Parameters.AddWithValue("p", user.PasswordHash);
        cmd.Parameters.AddWithValue("n", user.FullName);
        cmd.ExecuteNonQuery();
    }
}   