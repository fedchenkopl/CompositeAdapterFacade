namespace CompositeAdapterFacade.Composite;


public abstract class FileSystemItem
{
    public string Name { get; protected set; }

    protected FileSystemItem(string name)
    {
        Name = name;
    }
    public abstract long GetSize();
    public virtual void Add(FileSystemItem item)
    {
        throw new NotSupportedException("Операция не поддерживается для этого элемента");
    }

    public virtual void Remove(FileSystemItem item)
    {
        throw new NotSupportedException("Операция не поддерживается для этого элемента");
    }

    public virtual FileSystemItem? GetChild(int index)
    {
        throw new NotSupportedException("Операция не поддерживается для этого элемента");
    }
}