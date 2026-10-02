using TableroDirectorio.Models;
using TableroDirectorio.Services;

namespace TableroDirectorio.Presenters;

public class DashboardPresenter
{
    private readonly IDirectoryService _directoryService;
    private readonly IConfigService _configService;

    public DashboardPresenter(IDirectoryService directoryService, IConfigService configService)
    {
        _directoryService = directoryService;
        _configService = configService;
    }

    public IEnumerable<DirectoryItem> LoadItems() => _directoryService.GetAllItems();

    public IEnumerable<DirectoryItem> Search(string query) => _directoryService.SearchItems(query);

    public void OpenItem(DirectoryItem item) => _directoryService.OpenResource(item);

    public int AddItem(DirectoryItem item) => _directoryService.CreateItem(item);

    public bool EditItem(DirectoryItem item) => _directoryService.UpdateItem(item);

    public bool RemoveItem(int id) => _directoryService.DeleteItem(id);

    public string GetTitle() => _configService.GetAppTitle();
    public string GetSubtitle() => _configService.GetAppSubtitle();
    public string GetBanner() => _configService.GetBannerText();
    public string GetTheme() => _configService.GetTheme();
    public void SaveTheme(string theme) => _configService.SetTheme(theme);
}
