using Microsoft.Data.Sqlite;
using System.Data;

namespace TableroDirectorio.Data;

public class SqliteConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory()
    {
        var dbPath = System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "tablero.db");
        _connectionString = $"Data Source={dbPath}";
    }

    public IDbConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}
