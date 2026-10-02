namespace TableroDirectorio.Models;

public class DirectoryItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ResourceType { get; set; } = "WebPage"; // WebPage, PowerBI, Excel, Folder, Other
    public string Path { get; set; } = string.Empty;
    public string IconName { get; set; } = "chart";
    public string Category { get; set; } = "General";
    public string ColorHex { get; set; } = "#2563eb";
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
