namespace CompositeAdapterFacade.Composite;

/// <summary>
/// Базовый компонент (Component) для паттерна Компоновщик
/// </summary>
public abstract class FileSystemItem
{
    public string Name { get; protected set; }

    protected FileSystemItem(string name)
    {
        Name = name;
    }

    // Операции для всех элементов
    public abstract long GetSize();

    // Операции для компоновщиков (папок) - по умолчанию выбрасывают исключение
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