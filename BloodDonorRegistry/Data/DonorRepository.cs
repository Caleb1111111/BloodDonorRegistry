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
}