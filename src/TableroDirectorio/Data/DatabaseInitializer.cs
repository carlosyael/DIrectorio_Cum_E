using Dapper;
using Serilog;

namespace TableroDirectorio.Data;

public class DatabaseInitializer
{
    private readonly SqliteConnectionFactory _factory;

    public DatabaseInitializer(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public void Initialize()
    {
        try
        {
            using var connection = _factory.CreateConnection();
            connection.Open();

            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS DirectoryItems (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    ResourceType TEXT NOT NULL DEFAULT 'WebPage',
                    Path TEXT NOT NULL,
                    IconName TEXT DEFAULT 'chart',
                    Category TEXT DEFAULT 'General',
                    ColorHex TEXT DEFAULT '#2563eb',
                    SortOrder INTEGER DEFAULT 0,
                    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                    UpdatedAt TEXT NOT NULL DEFAULT (datetime('now'))
                );");

            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS AppConfig (
                    Key TEXT PRIMARY KEY,
                    Value TEXT
                );");

            // Seed data: check if table is empty
            var count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM DirectoryItems");
            if (count == 0)
            {
                SeedDirectoryItems(connection);
            }

            // Seed default config
            var configCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM AppConfig");
            if (configCount == 0)
            {
                SeedDefaultConfig(connection);
            }

            Log.Information("Database initialized successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to initialize database.");
            throw;
        }
    }

    private void SeedDirectoryItems(System.Data.IDbConnection connection)
    {
        var items = new[]
        {
            new {
                Title = "Indicadores de Ejecución Cumplimiento Ético",
                Description = "Seguimiento de indicadores operativos y estratégicos.",
                ResourceType = "PowerBI",
                Path = @"T:\GDCEAA\1.2 Revisión Ética\1.2.5 Estadísticas DCEAA\Indicadores de Ejecución Cumplimiento Ético 2026.pbix",
                IconName = "chart-line",
                Category = "Indicadores",
                ColorHex = "#3B82F6",
                SortOrder = 1
            },
            new {
                Title = "Matriz de Riesgo de Colaboradores",
                Description = "Evaluación y monitoreo de riesgos asociados a colaboradores.",
                ResourceType = "PowerBI",
                Path = @"T:\GDCEAA\1.2 Revisión Ética\1.2.18 Laboratorio de Datos\1.2.18.14 Reestrcturado Power Bi\Tablero Matriz\1.2.18.03 Matriz de riesgo de empleados\Matriz de Riesgo Colaboradores - Working2.pbix",
                IconName = "warning",
                Category = "Riesgos",
                ColorHex = "#EF4444",
                SortOrder = 2
            },
            new {
                Title = "Dashboard de Planificación de Actividades",
                Description = "Seguimiento de actividades y cumplimiento de planes.",
                ResourceType = "PowerBI",
                Path = @"T:\GDCEAA\1.2 Revisión Ética\1.2.18 Laboratorio de Datos\1.2.18.06 Dashboard Planificación de Actividades\Dashboard de Planificacion de Actividades.pbix",
                IconName = "calendar",
                Category = "Planificación",
                ColorHex = "#10B981",
                SortOrder = 3
            },
            new {
                Title = "Evaluación Transaccional",
                Description = "Análisis transaccional y monitoreo de colaboradores.",
                ResourceType = "PowerBI",
                Path = @"T:\GDCEAA\1.2 Revisión Ética\1.2.15 Evaluación de Riesgo\Evaluación transaccional - Nombre del colaborador 202606.pbix",
                IconName = "search",
                Category = "Evaluación",
                ColorHex = "#F59E0B",
                SortOrder = 4
            }
        };

        foreach (var item in items)
        {
            connection.Execute(@"
                INSERT INTO DirectoryItems (Title, Description, ResourceType, Path, IconName, Category, ColorHex, SortOrder)
                VALUES (@Title, @Description, @ResourceType, @Path, @IconName, @Category, @ColorHex, @SortOrder)",
                item);
        }

        Log.Information("Seeded {Count} directory items.", items.Length);
    }

    private void SeedDefaultConfig(System.Data.IDbConnection connection)
    {
        // Default master password is 'admin123' hashed with SHA256
        var defaultPasswordHash = Helpers.PasswordHasher.Hash("admin123");

        connection.Execute(@"
            INSERT INTO AppConfig (Key, Value) VALUES
            ('MasterPasswordHash', @Hash),
            ('AppTitle', 'Gerencia de Cumplimiento Ético'),
            ('AppSubtitle', 'Directorio de Tableros e Informaciones'),
            ('BannerText', '🛡️ Centro de Analítica y Gestión de Riesgos')",
            new { Hash = defaultPasswordHash });

        Log.Information("Seeded default configuration.");
    }
}
