using CompositeAdapterFacade.Adapter;

namespace CompositeAdapterFacade.Facade;

/// <summary>
/// Фасад (Facade) - предоставляет простой интерфейс для сложных операций
/// </summary>
public class SyncFacade
{
    private readonly IFileSystem _source;
    private readonly IFileSystem _target;

    public SyncFacade(IFileSystem source, IFileSystem target)
    {
        _source = source;
        _target = target;
    }

    /// <summary>
    /// Синхронизирует содержимое исходной папки с целевой
    /// </summary>
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
                    // В реальном приложении здесь нужно проверять, файл это или папка
                    // Для простоты считаем всё файлами
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

    /// <summary>
    /// Создает резервную копию исходной папки
    /// </summary>
    public void Backup(string sourcePath, string backupPath)
    {
        Console.WriteLine($"Создание резервной копии: {sourcePath} -> {backupPath}");

        // Добавляем временную метку к имени папки бэкапа
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fullBackupPath = CombinePath(backupPath, $"backup_{timestamp}");

        // Используем ту же логику синхронизации
        SyncFolder(sourcePath, fullBackupPath);

        Console.WriteLine("Резервное копирование завершено.\n");
    }

    private string CombinePath(string path1, string path2)
    {
        return $"{path1.TrimEnd('/')}/{path2.TrimStart('/')}";
    }
}