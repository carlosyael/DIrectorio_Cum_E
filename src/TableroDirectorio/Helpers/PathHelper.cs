namespace TableroDirectorio.Helpers;

public static class PathHelper
{
    public static string AppBaseDirectory =>
        AppDomain.CurrentDomain.BaseDirectory;

    public static string GetFullPath(string relativePath)
    {
        if (System.IO.Path.IsPathRooted(relativePath))
            return relativePath;

        return System.IO.Path.Combine(AppBaseDirectory, relativePath);
    }

    public static string LogsDirectory
    {
        get
        {
            var path = System.IO.Path.Combine(AppBaseDirectory, "logs");
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
