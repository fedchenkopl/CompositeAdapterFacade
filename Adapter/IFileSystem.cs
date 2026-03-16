namespace CompositeAdapterFacade.Adapter;

/// <summary>
/// Целевой интерфейс (Target), с которым работает клиент и фасад
/// </summary>
public interface IFileSystem
{
    List<string> ListItems(string path);
    byte[] ReadFile(string path);
    void WriteFile(string path, byte[] data);
    void DeleteItem(string path);
}