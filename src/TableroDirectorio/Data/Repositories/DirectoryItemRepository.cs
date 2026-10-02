using Dapper;
using Serilog;
using TableroDirectorio.Models;

namespace TableroDirectorio.Data.Repositories;

public class DirectoryItemRepository : IDirectoryItemRepository
{
    private readonly SqliteConnectionFactory _factory;

    public DirectoryItemRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public IEnumerable<DirectoryItem> GetAll()
    {
        try
        {
            using var conn = _factory.CreateConnection();
            return conn.Query<DirectoryItem>(
                "SELECT * FROM DirectoryItems ORDER BY SortOrder, Title");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving directory items.");
            return Enumerable.Empty<DirectoryItem>();
        }
    }

    public DirectoryItem? GetById(int id)
    {
        try
        {
            using var conn = _factory.CreateConnection();
            return conn.QuerySingleOrDefault<DirectoryItem>(
                "SELECT * FROM DirectoryItems WHERE Id = @Id", new { Id = id });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving directory item {Id}.", id);
            return null;
        }
    }

    public int Create(DirectoryItem item)
    {
        try
        {
            using var conn = _factory.CreateConnection();
            return conn.ExecuteScalar<int>(@"
                INSERT INTO DirectoryItems (Title, Description, ResourceType, Path, IconName, Category, ColorHex, SortOrder)
                VALUES (@Title, @Description, @ResourceType, @Path, @IconName, @Category, @ColorHex, @SortOrder);
                SELECT last_insert_rowid();", item);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating directory item.");
            return -1;
        }
    }

    public bool Update(DirectoryItem item)
    {
        try
        {
            using var conn = _factory.CreateConnection();
            var rows = conn.Execute(@"
                UPDATE DirectoryItems
                SET Title = @Title,
                    Description = @Description,
                    ResourceType = @ResourceType,
                    Path = @Path,
                    IconName = @IconName,
                    Category = @Category,
                    ColorHex = @ColorHex,
                    SortOrder = @SortOrder,
                    UpdatedAt = datetime('now')
                WHERE Id = @Id", item);
            return rows > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating directory item {Id}.", item.Id);
            return false;
        }
    }

    public bool Delete(int id)
    {
        try
        {
            using var conn = _factory.CreateConnection();
            var rows = conn.Execute(
                "DELETE FROM DirectoryItems WHERE Id = @Id", new { Id = id });
            return rows > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting directory item {Id}.", id);
            return false;
        }
    }

    public IEnumerable<DirectoryItem> Search(string query)
    {
        try
        {
            using var conn = _factory.CreateConnection();
            return conn.Query<DirectoryItem>(@"
                SELECT * FROM DirectoryItems
                WHERE Title LIKE @Q OR Description LIKE @Q OR Category LIKE @Q
                ORDER BY SortOrder, Title",
                new { Q = $"%{query}%" });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error searching directory items.");
            return Enumerable.Empty<DirectoryItem>();
        }
    }
}
