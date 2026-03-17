using CompositeAdapterFacade.Adapter;

namespace CompositeAdapterFacade.Facade;
public class SyncFacade
{
    private readonly IFileSystem _source;
    private readonly IFileSystem _target;

    public SyncFacade(IFileSystem source, IFileSystem target)
    {
        _source = source;
        _target = target;
    }
    public void SyncFolder(string sourcePath, string targetPath)
    {
        Console.WriteLine($"Начало синхронизации: {sourcePath} -> {targetPath}");

        try
        {
            var items = _source.ListItems(sourcePath);

            foreach (var item in items)
            {
                var sourceItemPath = CombinePath(sourcePath, item);
                var targetItemPath = CombinePath(targetPath, item);

                try
                {
                    var data = _source.ReadFile(sourceItemPath);
                    _target.WriteFile(targetItemPath, data);
                    Console.WriteLine($"  + {item} синхронизирован");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ! Ошибка при синхронизации {item}: {ex.Message}");
                }
            }

            Console.WriteLine($"Синхронизация завершена.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка синхронизации: {ex.Message}");
        }
    }
    public void Backup(string sourcePath, string backupPath)
    {
        Console.WriteLine($"Создание резервной копии: {sourcePath} -> {backupPath}");

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fullBackupPath = CombinePath(backupPath, $"backup_{timestamp}");

        SyncFolder(sourcePath, fullBackupPath);

        Console.WriteLine("Резервное копирование завершено.\n");
    }

    private string CombinePath(string path1, string path2)
    {
        return $"{path1.TrimEnd('/')}/{path2.TrimStart('/')}";
    }
}