using System;
using System.IO;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к папке:");
            string folderPath = Console.ReadLine();

            ClearUnusedFolders(folderPath, true);
        }

        static void ClearUnusedFolders(string folderPath, bool deleteRoot)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(folderPath);

                if (!directory.Exists)
                {
                    Console.WriteLine("Указанная папка не существует.");
                    return;
                }

                foreach (var subDir in directory.GetDirectories())
                {
                    ClearUnusedFolders(subDir.FullName, false);
                }

                foreach (var file in directory.GetFiles())
                {
                    if ((DateTime.Now - file.LastWriteTime) > TimeSpan.FromMinutes(30))
                    {
                        file.Delete();
                        Console.WriteLine($"Файл был удален: {file.Name}");
                    }
                }

                if ((DateTime.Now - directory.LastWriteTime) > TimeSpan.FromMinutes(30) && !deleteRoot)
                {
                    directory.Delete();
                    Console.WriteLine($"Папка была удалена: {directory.FullName}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Отсутствуют права доступа к папке: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Неверный путь: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке папки: {ex.Message}");
            }
        }
    }
}