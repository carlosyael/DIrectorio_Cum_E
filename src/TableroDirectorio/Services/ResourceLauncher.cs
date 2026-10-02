using System.Diagnostics;
using Serilog;

namespace TableroDirectorio.Services;

public static class ResourceLauncher
{
    public static void Open(string path, string resourceType)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Log.Warning("Attempted to open resource with empty path.");
                return;
            }

            var psi = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            };

            // For URLs, ensure we use the shell
            if (resourceType == "WebPage" || path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                psi.FileName = path;
            }

            Process.Start(psi);
            Log.Information("Opened resource: {Path} (Type: {Type})", path, resourceType);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to open resource: {Path}", path);
            MessageBox.Show(
                $"No se pudo abrir el recurso:\n{path}\n\nError: {ex.Message}",
                "Error al Abrir",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
