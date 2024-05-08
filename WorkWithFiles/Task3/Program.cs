using System;
using System.IO;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к папке, которую необходимо отчистить:");
            string folderPath = Console.ReadLine();

            try
            {
                long initialFolderSize = CalculateFolderSize(folderPath);
                Console.WriteLine($"Исходный размер папки: {initialFolderSize} байтов.");

                ClearUnusedFolders(folderPath, true);

                long freedSpace = initialFolderSize - CalculateFolderSize(folderPath);
                Console.WriteLine($"Освобождено: {freedSpace} байтов");

                long finalFolderSize = CalculateFolderSize(folderPath);
                Console.WriteLine($"Текущий размер папки: {finalFolderSize} байтов");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static long CalculateFolderSize(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("Указанная папка не существует.");
            }

            long size = 0;
            DirectoryInfo directory = new DirectoryInfo(folderPath);

            try
            {
                foreach (var file in directory.GetFiles())
                {
                    size += file.Length;
                }

                foreach (var subDir in directory.GetDirectories())
                {
                    size += CalculateFolderSize(subDir.FullName);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Отсутствуют права доступа к папке: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке папки: {ex.Message}");
            }

            return size;
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