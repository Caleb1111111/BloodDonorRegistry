using System.Text.Json;
using Npgsql;

namespace BloodDonorRegistry.Data;

public static class DbConnectionFactory
{
    private static readonly string ConnectionString = LoadConnectionString();

    private static string LoadConnectionString()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(path))
            throw new FileNotFoundException(
                "appsettings.json not found. Copy appsettings.sample.json to appsettings.json and enter your password.", path);

        using var doc = JsonDocument.Parse(File.ReadAllText(path));
        return doc.RootElement
                  .GetProperty("ConnectionStrings")
                  .GetProperty("Default")
                  .GetString()!;
    }

    public static NpgsqlConnection Create() => new NpgsqlConnection(ConnectionString);
}