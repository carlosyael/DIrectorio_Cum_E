using Serilog;
using TableroDirectorio.Helpers;
using TableroDirectorio.UI;

namespace TableroDirectorio;

internal static class Program
{
    [STAThread]
    static void Main()
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