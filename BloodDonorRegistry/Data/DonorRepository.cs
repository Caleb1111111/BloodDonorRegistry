using BloodDonorRegistry.Models;
using Npgsql;
using NpgsqlTypes;

namespace BloodDonorRegistry.Data;

public class DonorRepository : IDonorRepository
{
    public List<Donor> Search(string text, string? bloodGroup)
    {
        var result = new List<Donor>();

        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            SELECT donor_id, national_id, full_name, phone, date_of_birth, district, blood_group
            FROM donors
            WHERE (@t = '' OR full_name ILIKE '%' || @t || '%' OR national_id ILIKE '%' || @t || '%')
              AND (@bg IS NULL OR blood_group = @bg)
            ORDER BY full_name", conn);
        cmd.Parameters.Add("t", NpgsqlDbType.Varchar).Value = text;
        cmd.Parameters.Add("bg", NpgsqlDbType.Varchar).Value = (object?)bloodGroup ?? DBNull.Value;

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            result.Add(new Donor
            {
                DonorId = r.GetInt32(0),
                NationalId = r.GetString(1),
                FullName = r.GetString(2),
                Phone = r.GetString(3),
                DateOfBirth = r.GetDateTime(4),
                District = r.GetString(5),
                BloodGroup = r.GetString(6)
            });
        }
        return result;
    }

    public void Add(Donor donor)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            INSERT INTO donors (national_id, full_name, phone, date_of_birth, district, blood_group)
            VALUES (@n, @f, @p, @d, @di, @bg)", conn);
        FillParameters(cmd, donor);
        cmd.ExecuteNonQuery();
    }

    public void Update(Donor donor)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            UPDATE donors
            SET national_id = @n, full_name = @f, phone = @p,
                date_of_birth = @d, district = @di, blood_group = @bg
            WHERE donor_id = @id", conn);
        FillParameters(cmd, donor);
        cmd.Parameters.AddWithValue("id", donor.DonorId);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int donorId)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand("DELETE FROM donors WHERE donor_id = @id", conn);
        cmd.Parameters.AddWithValue("id", donorId);
        cmd.ExecuteNonQuery();
    }

    public bool NationalIdExists(string nationalId, int excludeDonorId)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT EXISTS (SELECT 1 FROM donors WHERE national_id = @n AND donor_id <> @id)", conn);
        cmd.Parameters.AddWithValue("n", nationalId);
        cmd.Parameters.AddWithValue("id", excludeDonorId);
        return (bool)cmd.ExecuteScalar()!;
    }

    public bool HasDonations(int donorId)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT EXISTS (SELECT 1 FROM donations WHERE donor_id = @id)", conn);
        cmd.Parameters.AddWithValue("id", donorId);
        return (bool)cmd.ExecuteScalar()!;
    }

    private static void FillParameters(NpgsqlCommand cmd, Donor d)
    {
        cmd.Parameters.AddWithValue("n", d.NationalId);
        cmd.Parameters.AddWithValue("f", d.FullName);
        cmd.Parameters.AddWithValue("p", d.Phone);
        cmd.Parameters.Add("d", NpgsqlDbType.Date).Value = d.DateOfBirth.Date;
        cmd.Parameters.AddWithValue("di", d.District);
        cmd.Parameters.AddWithValue("bg", d.BloodGroup);
    }
}