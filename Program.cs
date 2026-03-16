using CompositeAdapterFacade.Composite;
using CompositeAdapterFacade.Adapter;
using CompositeAdapterFacade.Facade;

namespace CompositeAdapterFacade;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Лабораторная работа №5: Компоновщик, Адаптер, Фасад ===\n");

        // ========== ЭТАП 1: Компоновщик (Composite) ==========
        Console.WriteLine("--- 1. Создание структуры файлов (Composite) ---");

        // Создаем папки и файлы
        var root = new Folder("root");
        var docs = new Folder("docs");
        var pictures = new Folder("pictures");
        var file1 = new Composite.File("readme.txt", 1024);
        var file2 = new Composite.File("config.ini", 512);
        var file3 = new Composite.File("photo.jpg", 2048);
        var file4 = new Composite.File("note.txt", 256);

        // Строим дерево
        docs.Add(file1);
        docs.Add(file2);
        pictures.Add(file3);
        root.Add(docs);
        root.Add(pictures);
        root.Add(file4);

        // Демонстрация работы Composite
        Console.WriteLine($"Размер root: {root.GetSize()} байт");
        Console.WriteLine($"Размер docs: {docs.GetSize()} байт");
        Console.WriteLine($"Содержимое root:");
        PrintContents(root, 1);

        // ========== ЭТАП 2: Адаптер (Adapter) ==========
        Console.WriteLine("\n--- 2. Работа через адаптер (IFileSystem) ---");

        // Создаем адаптер для нашей структуры
        IFileSystem fileSystem = new FileSystemAdapter(root);

        // Получаем список корневой папки
        var rootItems = fileSystem.ListItems("root");
        Console.WriteLine($"Содержимое root (через адаптер): {string.Join(", ", rootItems)}");

        // Читаем файл
        if (rootItems.Contains("readme.txt"))
        {
            var data = fileSystem.ReadFile("root/docs/readme.txt");
            Console.WriteLine($"Прочитан файл readme.txt, размер данных: {data.Length} байт");
        }

        // Создаем целевую файловую систему для копирования
        var backupRoot = new Folder("backup");
        IFileSystem backupSystem = new FileSystemAdapter(backupRoot);

        // ========== ЭТАП 3: Фасад (Facade) ==========
        Console.WriteLine("\n--- 3. Использование фасада для синхронизации ---");

        var facade = new SyncFacade(fileSystem, backupSystem);
        facade.SyncFolder("root", "backup");

        // Проверяем результат
        Console.WriteLine($"\nРазмер оригинального root: {root.GetSize()} байт");
        Console.WriteLine($"Размер backup: {backupRoot.GetSize()} байт");
    }

    // Вспомогательный метод для рекурсивного вывода структуры
    static void PrintContents(Folder folder, int level)
    {
        var indent = new string(' ', level * 2);
        foreach (var item in folder.GetChildren())
        {
            if (item is Composite.File file)
            {
                Console.WriteLine($"{indent}- {file.Name} (файл, {file.GetSize()} байт)");
            }
            else if (item is Folder subFolder)
            {
                Console.WriteLine($"{indent}+ {subFolder.Name} (папка)");
                PrintContents(subFolder, level + 1);
            }
        }
    }
}