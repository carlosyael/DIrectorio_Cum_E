using TableroDirectorio.Services;

namespace TableroDirectorio.Presenters;

public class ConfigPresenter
{
    private readonly IConfigService _configService;

    public ConfigPresenter(IConfigService configService)
    {
        _configService = configService;
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
}
