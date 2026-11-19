using Npgsql;

namespace BloodDonorRegistry.Data;

public class SettingsRepository : ISettingsRepository
{
    private const string Key = "waiting_period_months";
    private const int DefaultMonths = 3;

    public int GetWaitingMonths()
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT setting_value FROM settings WHERE setting_key = @k", conn);
        cmd.Parameters.AddWithValue("k", Key);

        var value = cmd.ExecuteScalar() as string;
        return int.TryParse(value, out int months) ? months : DefaultMonths;
    }

    public void SetWaitingMonths(int months)
    {
        using var conn = DbConnectionFactory.Create();
        conn.Open();
        using var cmd = new NpgsqlCommand(@"
            INSERT INTO settings (setting_key, setting_value) VALUES (@k, @v)
            ON CONFLICT (setting_key) DO UPDATE SET setting_value = EXCLUDED.setting_value", conn);
        cmd.Parameters.AddWithValue("k", Key);
        cmd.Parameters.AddWithValue("v", months.ToString());
        cmd.ExecuteNonQuery();
    }
}