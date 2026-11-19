using BloodDonorRegistry.Models;
using Npgsql;
using NpgsqlTypes;

namespace BloodDonorRegistry.Data;

public class DonationRepository : IDonationRepository
{
    public List<DonationRecord> GetAll(int? donorId)
    {
        var result = new List<DonationRecord>();

        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            SELECT d.donation_id, d.donor_id, dn.full_name, dn.blood_group,
                   d.donation_date, d.place, d.amount_ml
            FROM donations d
            JOIN donors dn ON dn.donor_id = d.donor_id
            WHERE (@id IS NULL OR d.donor_id = @id)
            ORDER BY d.donation_date DESC, d.donation_id DESC", conn);
        cmd.Parameters.Add("id", NpgsqlDbType.Integer).Value = (object?)donorId ?? DBNull.Value;

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            result.Add(new DonationRecord
            {
                DonationId = r.GetInt32(0),
                DonorId = r.GetInt32(1),
                DonorName = r.GetString(2),
                BloodGroup = r.GetString(3),
                DonationDate = r.GetDateTime(4),
                Place = r.GetString(5),
                AmountMl = r.GetInt32(6)
            });
        }
        return result;
    }

    public void Add(Donation donation)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            INSERT INTO donations (donor_id, donation_date, place, amount_ml)
            VALUES (@id, @date, @place, @ml)", conn);
        cmd.Parameters.AddWithValue("id", donation.DonorId);
        cmd.Parameters.Add("date", NpgsqlDbType.Date).Value = donation.DonationDate.Date;
        cmd.Parameters.AddWithValue("place", donation.Place);
        cmd.Parameters.AddWithValue("ml", donation.AmountMl);
        cmd.ExecuteNonQuery();
    }
}