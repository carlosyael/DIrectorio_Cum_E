using Serilog;
using TableroDirectorio.Data;
using TableroDirectorio.Data.Repositories;
using TableroDirectorio.Helpers;
using TableroDirectorio.Services;
using TableroDirectorio.UI;

namespace TableroDirectorio;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        // Configure Serilog to write to a local log file
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(
                Path.Combine(PathHelper.LogsDirectory, "tablero-.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        try
        {
            if (args.Length > 0 && args[0].Equals("--reset-password", StringComparison.OrdinalIgnoreCase))
            {
                var newPassword = args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]) ? args[1] : "admin123";
                var factory = new SqliteConnectionFactory();
                var initializer = new DatabaseInitializer(factory);
                initializer.Initialize();
                var repo = new ConfigRepository(factory);
                var service = new ConfigService(repo);
                service.ChangeMasterPassword(newPassword);
                MessageBox.Show(
                    $"La contraseña de administrador se ha restablecido exitosamente a:\n\n{newPassword}\n\nYa puedes ingresar al módulo de Administración.",
                    "Contraseña Restablecida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            Log.Information("Application starting...");

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly.");
            MessageBox.Show(
                $"Error fatal al iniciar la aplicación:\n{ex.Message}",
                "Error Crítico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}