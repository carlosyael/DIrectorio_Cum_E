using Serilog;
using TableroDirectorio.Data.Repositories;
using TableroDirectorio.Models;

namespace TableroDirectorio.Services;

public class DirectoryService : IDirectoryService
{
    private readonly IDirectoryItemRepository _repository;

    public DirectoryService(IDirectoryItemRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<DirectoryItem> GetAllItems() => _repository.GetAll();

    public DirectoryItem? GetItem(int id) => _repository.GetById(id);

    public int CreateItem(DirectoryItem item)
    {
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        return _repository.Create(item);
    }

    public bool UpdateItem(DirectoryItem item)
    {
        item.UpdatedAt = DateTime.UtcNow;
        return _repository.Update(item);
    }

    public bool DeleteItem(int id) => _repository.Delete(id);

    public IEnumerable<DirectoryItem> SearchItems(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return GetAllItems();
        return _repository.Search(query);
    }

    public void OpenResource(DirectoryItem item)
    {
        ResourceLauncher.Open(item.Path, item.ResourceType);
    }
}
