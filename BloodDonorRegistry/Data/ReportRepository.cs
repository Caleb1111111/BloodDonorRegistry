using Npgsql;
using NpgsqlTypes;

namespace BloodDonorRegistry.Data;

public class ReportRepository : IReportRepository
{
    public int CountDonors() => Scalar("SELECT COUNT(*) FROM donors");

    public int CountDonations() => Scalar("SELECT COUNT(*) FROM donations");

    public int CountDonationsThisMonth() => Scalar(
        "SELECT COUNT(*) FROM donations " +
        "WHERE date_trunc('month', donation_date) = date_trunc('month', CURRENT_DATE)");

    public Dictionary<string, int> DonorsPerBloodGroup()
    {
        var result = new Dictionary<string, int>();
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT blood_group, COUNT(*) FROM donors GROUP BY blood_group", conn);
        using var r = cmd.ExecuteReader();
        while (r.Read()) result[r.GetString(0)] = Convert.ToInt32(r.GetInt64(1));
        return result;
    }

    public Dictionary<DateTime, int> DonationsPerMonth(DateTime fromMonth)
    {
        var result = new Dictionary<DateTime, int>();
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            SELECT date_trunc('month', donation_date)::date AS m, COUNT(*)
            FROM donations
            WHERE donation_date >= @from
            GROUP BY m
            ORDER BY m", conn);
        cmd.Parameters.Add("from", NpgsqlDbType.Date).Value = fromMonth.Date;
        using var r = cmd.ExecuteReader();
        while (r.Read()) result[r.GetDateTime(0)] = Convert.ToInt32(r.GetInt64(1));
        return result;
    }

    private static int Scalar(string sql)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}