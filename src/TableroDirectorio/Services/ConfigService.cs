using Serilog;
using TableroDirectorio.Data.Repositories;
using TableroDirectorio.Helpers;

namespace TableroDirectorio.Services;

public class ConfigService : IConfigService
{
    private readonly IConfigRepository _repository;

    public ConfigService(IConfigRepository repository)
    {
        _repository = repository;
    }

    public string GetAppTitle() =>
        _repository.GetValue("AppTitle") ?? "Gerencia de Cumplimiento Ético";

    public string GetAppSubtitle() =>
        _repository.GetValue("AppSubtitle") ?? "Directorio de Tableros e Informaciones";

    public string GetBannerText() =>
        _repository.GetValue("BannerText") ?? "🛡️ Centro de Analítica y Gestión de Riesgos";

    public bool ValidateMasterPassword(string password)
    {
        var storedHash = _repository.GetValue("MasterPasswordHash");
        if (string.IsNullOrEmpty(storedHash))
        {
            Log.Warning("No master password hash found in config.");
            return false;
        }
        return PasswordHasher.Verify(password, storedHash);
    }

    public void ChangeMasterPassword(string newPassword)
    {
        var hash = PasswordHasher.Hash(newPassword);
        _repository.SetValue("MasterPasswordHash", hash);
        Log.Information("Master password changed.");
    }

    public string? GetValue(string key) => _repository.GetValue(key);

    public void SetValue(string key, string value) => _repository.SetValue(key, value);
}
