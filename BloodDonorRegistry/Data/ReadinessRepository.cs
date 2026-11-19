using BloodDonorRegistry.Models;
using Npgsql;
using NpgsqlTypes;

namespace BloodDonorRegistry.Data;

public class ReadinessRepository : IReadinessRepository
{
    public List<DonorReadiness> GetDonorsWithLastDonation(string text, string? bloodGroup)
    {
        var result = new List<DonorReadiness>();

        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            SELECT d.donor_id, d.national_id, d.full_name, d.phone, d.date_of_birth,
                   d.district, d.blood_group, MAX(n.donation_date)
            FROM donors d
            LEFT JOIN donations n ON n.donor_id = d.donor_id
            WHERE (@t = '' OR d.full_name ILIKE '%' || @t || '%' OR d.national_id ILIKE '%' || @t || '%')
              AND (@bg IS NULL OR d.blood_group = @bg)
            GROUP BY d.donor_id
            ORDER BY d.full_name", conn);
        cmd.Parameters.Add("t", NpgsqlDbType.Varchar).Value = text;
        cmd.Parameters.Add("bg", NpgsqlDbType.Varchar).Value = (object?)bloodGroup ?? DBNull.Value;

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            result.Add(new DonorReadiness
            {
                Donor = new Donor
                {
                    DonorId = r.GetInt32(0),
                    NationalId = r.GetString(1),
                    FullName = r.GetString(2),
                    Phone = r.GetString(3),
                    DateOfBirth = r.GetDateTime(4),
                    District = r.GetString(5),
                    BloodGroup = r.GetString(6)
                },
                LastDonation = r.IsDBNull(7) ? null : r.GetDateTime(7)
            });
        }
        return result;
    }
}