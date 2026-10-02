using TableroDirectorio.Models;
using TableroDirectorio.Services;

namespace TableroDirectorio.Presenters;

public class ConfigPresenter
{
    private readonly IConfigService _configService;
    private readonly IDirectoryService _directoryService;

    public ConfigPresenter(IConfigService configService, IDirectoryService directoryService)
    {
        _configService = configService;
        _directoryService = directoryService;
    }

    public bool Authenticate(string password) =>
        _configService.ValidateMasterPassword(password);

    public void ChangePassword(string newPassword) =>
        _configService.ChangeMasterPassword(newPassword);

    public string GetAppTitle() => _configService.GetAppTitle();
    public string GetAppSubtitle() => _configService.GetAppSubtitle();
    public string GetBannerText() => _configService.GetBannerText();

    public void SaveAppTitle(string title) => _configService.SetValue("AppTitle", title);
    public void SaveAppSubtitle(string subtitle) => _configService.SetValue("AppSubtitle", subtitle);
    public void SaveBannerText(string text) => _configService.SetValue("BannerText", text);

    public string GetTheme() => _configService.GetTheme();
    public void SaveTheme(string theme) => _configService.SetTheme(theme);

    // Board Management
    public IEnumerable<DirectoryItem> GetBoards() => _directoryService.GetAllItems();
    public int CreateBoard(DirectoryItem item) => _directoryService.CreateItem(item);
    public bool UpdateBoard(DirectoryItem item) => _directoryService.UpdateItem(item);
    public bool DeleteBoard(int id) => _directoryService.DeleteItem(id);
    public void ReorderBoards(IEnumerable<(int Id, int SortOrder)> items) => _directoryService.UpdateOrder(items);
}
