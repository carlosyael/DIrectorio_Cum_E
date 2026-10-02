namespace TableroDirectorio.Data.Repositories;

public interface IConfigRepository
{
    string? GetValue(string key);
    void SetValue(string key, string value);
    Dictionary<string, string> GetAll();
}
