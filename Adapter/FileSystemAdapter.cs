using CompositeAdapterFacade.Composite;
namespace CompositeAdapterFacade.Adapter;
public class FileSystemAdapter : IFileSystem
{
    private readonly FileSystemItem _root;

    public FileSystemAdapter(FileSystemItem root)
    {
        _root = root;
    }

    public List<string> ListItems(string path)
    {
        var item = FindItem(path);

        if (item is Folder folder)
        {
            return folder.GetChildren().Select(c => c.Name).ToList();
        }

        throw new InvalidOperationException($"'{path}' не является папкой");
    }

    public byte[] ReadFile(string path)
    {
        var item = FindItem(path);

        if (item is Composite.File file)
        {
            return new byte[file.GetSize()];
        }

        throw new InvalidOperationException($"'{path}' не является файлом");
    }

    public void WriteFile(string path, byte[] data)
    {
        var item = FindItem(path);

        if (item is Composite.File file)
        {
            Console.WriteLine($"Записано {data.Length} байт в файл {file.Name}");
        }
        else
        {
            throw new InvalidOperationException($"'{path}' не является файлом");
        }
    }

    public void DeleteItem(string path)
    {
        var parentPath = GetParentPath(path);
        var itemName = GetItemName(path);

        var parent = FindItem(parentPath) as Folder;
        if (parent == null)
        {
            throw new InvalidOperationException($"Родительская папка не найдена для '{path}'");
        }

        var itemToDelete = FindItem(path);
        parent.Remove(itemToDelete);
        Console.WriteLine($"Удален элемент: {path}");
    }
    private FileSystemItem FindItem(string path)
    {
        if (string.IsNullOrEmpty(path) || path == _root.Name)
            return _root;

        var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var current = _root;

        foreach (var part in parts)
        {
            if (current is Folder folder)
            {
                var next = folder.GetChildren().FirstOrDefault(c => c.Name == part);
                if (next == null)
                    throw new FileNotFoundException($"Элемент '{part}' не найден в пути '{path}'");
                current = next;
            }
            else
            {
                throw new InvalidOperationException($"'{current.Name}' не является папкой");
            }
        }

        return current;
    }

    private string GetParentPath(string path)
    {
        var lastSlashIndex = path.LastIndexOf('/');
        if (lastSlashIndex <= 0)
            return _root.Name;
        return path.Substring(0, lastSlashIndex);
    }

    private string GetItemName(string path)
    {
        var lastSlashIndex = path.LastIndexOf('/');
        if (lastSlashIndex < 0)
            return path;
        return path.Substring(lastSlashIndex + 1);
    }
}