namespace TableroDirectorio.Services;

public interface IConfigService
{
    string GetAppTitle();
    string GetAppSubtitle();
    string GetBannerText();
    bool ValidateMasterPassword(string password);
    void ChangeMasterPassword(string newPassword);
    string? GetValue(string key);
    void SetValue(string key, string value);
    string GetTheme();
    void SetTheme(string themeName);
}
