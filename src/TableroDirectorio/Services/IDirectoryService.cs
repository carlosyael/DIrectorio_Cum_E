using TableroDirectorio.Models;

namespace TableroDirectorio.Services;

public interface IDirectoryService
{
    IEnumerable<DirectoryItem> GetAllItems();
    DirectoryItem? GetItem(int id);
    int CreateItem(DirectoryItem item);
    bool UpdateItem(DirectoryItem item);
    bool DeleteItem(int id);
    IEnumerable<DirectoryItem> SearchItems(string query);
    void OpenResource(DirectoryItem item);
}
