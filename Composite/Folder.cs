namespace CompositeAdapterFacade.Composite;

/// <summary>
/// Составной узел (Composite) - папка
/// </summary>
public class Folder : FileSystemItem
{
    private readonly List<FileSystemItem> _children = new();

    public Folder(string name) : base(name)
    {
    }

    public override long GetSize()
    {
        // Рекурсивно суммируем размеры всех дочерних элементов
        return _children.Sum(child => child.GetSize());
    }

    public override void Add(FileSystemItem item)
    {
        _children.Add(item);
    }

    public override void Remove(FileSystemItem item)
    {
        _children.Remove(item);
    }

    public override FileSystemItem? GetChild(int index)
    {
        if (index >= 0 && index < _children.Count)
            return _children[index];
        return null;
    }

    // Дополнительный метод для получения всех детей (нужен для адаптера)
    public IReadOnlyList<FileSystemItem> GetChildren() => _children;
}