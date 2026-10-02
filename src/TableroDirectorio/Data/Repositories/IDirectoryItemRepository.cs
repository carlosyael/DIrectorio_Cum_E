using TableroDirectorio.Models;

namespace TableroDirectorio.Data.Repositories;

public interface IDirectoryItemRepository
{
    IEnumerable<DirectoryItem> GetAll();
    DirectoryItem? GetById(int id);
    int Create(DirectoryItem item);
    bool Update(DirectoryItem item);
    bool Delete(int id);
    IEnumerable<DirectoryItem> Search(string query);
}
