namespace CompositeAdapterFacade.Composite;
public class Folder : FileSystemItem
{
    private readonly List<FileSystemItem> _children = new();

    public Folder(string name) : base(name)
    {
    }

    public override long GetSize()
    {
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
    public IReadOnlyList<FileSystemItem> GetChildren() => _children;
}