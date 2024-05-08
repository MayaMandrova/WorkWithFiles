using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Напишите путь к файлу с бинарными данными:");
            string binaryFilePath = Console.ReadLine();
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string studentsDirectory = Path.Combine(desktopPath, "Students");

            if (!File.Exists(binaryFilePath))
            {
                Console.WriteLine("Файл с бинарными данными не найден.");
                return;
            }

            if (!Directory.Exists(studentsDirectory))
                Directory.CreateDirectory(studentsDirectory);

            var studentsByGroup = new Dictionary<string, List<Student>>();

            try
            {
                using (var reader = new BinaryReader(File.Open(binaryFilePath, FileMode.Open)))
                {
                    while (reader.PeekChar() != -1)
                    {
                        Student student = new Student
                        {
                            Name = reader.ReadString(),
                            Group = reader.ReadString(),
                            DateOfBirth = new DateTime(reader.ReadInt64()),
                            AverageGrade = reader.ReadDecimal()
                        };

                        if (studentsByGroup.ContainsKey(student.Group))
                            studentsByGroup[student.Group].Add(student);
                        else
                            studentsByGroup.Add(student.Group, new List<Student> { student });
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка при работе с файлом: {ex.Message}");
            }

            foreach (var group in studentsByGroup)
            {
                string groupFilePath = Path.Combine(studentsDirectory, $"{group.Key}.txt");

                using (var writer = new StreamWriter(groupFilePath))
                {
                    foreach (var student in group.Value)
                        writer.WriteLine($"{student.Name}, {student.DateOfBirth.ToString("dd.MM.yyyy")}, {student.AverageGrade}");
                }
            }
            Console.WriteLine("Данные успешно сконвертированы в текстовый формат и распределены по группам.");
        }
    }
}