using System;
using System.IO;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к папке:");
            string folderPath = Console.ReadLine();

            try
            {
                long folderSize = CalculateFolderSize(folderPath);
                Console.WriteLine($"Размер папки в байтах: {folderSize}");
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
    }
}