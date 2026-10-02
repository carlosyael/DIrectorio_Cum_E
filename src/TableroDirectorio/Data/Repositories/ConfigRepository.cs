using Dapper;
using Serilog;

namespace TableroDirectorio.Data.Repositories;

public class ConfigRepository : IConfigRepository
{
    private readonly SqliteConnectionFactory _factory;

    public ConfigRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public string? GetValue(string key)
    {
        try
        {
            using var conn = _factory.CreateConnection();
            return conn.ExecuteScalar<string>(
                "SELECT Value FROM AppConfig WHERE Key = @Key",
                new { Key = key });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting config value for {Key}.", key);
            return null;
        }
    }

    public void SetValue(string key, string value)
    {
        try
        {
            using var conn = _factory.CreateConnection();
            conn.Execute(@"
                INSERT INTO AppConfig (Key, Value) VALUES (@Key, @Value)
                ON CONFLICT(Key) DO UPDATE SET Value = @Value",
                new { Key = key, Value = value });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error setting config value for {Key}.", key);
        }
    }

    public Dictionary<string, string> GetAll()
    {
        try
        {
            using var conn = _factory.CreateConnection();
            var items = conn.Query<Models.AppConfig>("SELECT * FROM AppConfig");
            return items.ToDictionary(x => x.Key, x => x.Value);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting all config values.");
            return new Dictionary<string, string>();
        }
    }
}
