using Generator.Core;
using Generator.Importer.Core;
using Generator.Importer.DAL;
using Generator.Joiner.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Generator.Launcher
{
    public class Launcher
    {
        private static readonly Stopwatch _stopwatch = new Stopwatch();
        private static FileGenerator? _fileGenerator;
        private static FileJoiner? _fileJoiner;
        private static FileImporter? _fileImporter;
        public static void LaunchApplication()
        {
            int commandNumber = 0;
            while (commandNumber != 4)
            {
                Console.WriteLine("Enter the command number:\n" +
                    "1.Generate files\n" +
                    "2.Join files\n" +
                    "3.Import files\n" +
                    "4.Exit");
                commandNumber = Convert.ToInt32(Console.ReadLine());
                switch(commandNumber)
                {
                    case 1:
                        {
                            LaunchGenerator();
                            break;
                        }
                    case 2:
                        {
                            LaunchJoiner();
                            break;
                        }
                    case 3:
                        {
                            LaunchImporter();
                            break;
                        }
                    case 4:
                        {
                            commandNumber = 4;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Undefined command.");
                            break;
                        }
                }
            }
        }

        private static void LaunchGenerator()
        {
            string? path;
            do
            {
                Console.WriteLine("Input path for generation\n" +
                "Example: C:\\User");
                path = Console.ReadLine();
            } while (string.IsNullOrEmpty(path));

            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                } catch (ArgumentException)
                {
                    Console.WriteLine("Wrong path. Path example: C:\\User");
                } catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }

            Console.WriteLine("Input count of files for generation:");
            int countForGeneration = Convert.ToInt32(Console.ReadLine());

            if (_fileGenerator is null)
            {
                _fileGenerator = new FileGenerator();
            }

            Console.WriteLine("Generation started...");
            _stopwatch.Restart();
            _fileGenerator.GenerateFiles(path, countForGeneration);
            _stopwatch.Stop();
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                _stopwatch.Elapsed.Hours,
                _stopwatch.Elapsed.Minutes,
                _stopwatch.Elapsed.Seconds,
                _stopwatch.Elapsed.Milliseconds / 10);
            Console.WriteLine($"{countForGeneration} files generated. Elapsed time: {elapsedTime}");
        }

        private static void LaunchJoiner()
        {
            string? path;
            do
            {
                Console.WriteLine("Input path for join\n" +
                "Example: C:\\User");
                path = Console.ReadLine();
            } while (string.IsNullOrEmpty(path));

            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Directory not found. Try to input the path again.");
            }
            if (!Directory.Exists(path + "\\joined"))
            {
                Directory.CreateDirectory(path + "\\joined");
            }

            if (_fileJoiner is null)
            {
                _fileJoiner = new FileJoiner();
            }

            string? deleteString;
            do
            {
                Console.WriteLine("Input delete string(optional) or press ENTER to skip:");
                deleteString = Console.ReadLine();
            } while (deleteString is null);

            Console.WriteLine("Join started...");
            _stopwatch.Restart();
            int deletedStringsCount = _fileJoiner.JoinFiles(path, deleteString);
            _stopwatch.Stop();
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                _stopwatch.Elapsed.Hours,
                _stopwatch.Elapsed.Minutes,
                _stopwatch.Elapsed.Seconds,
                _stopwatch.Elapsed.Milliseconds / 10);
            Console.WriteLine($"Files joined. Deleted strings: {deletedStringsCount}. Elapsed time: {elapsedTime}.");
        }

        private static void LaunchImporter()
        {
            string? path;
            do
            {
                Console.WriteLine("Input path for import\n" +
                "Example: C:\\User");
                path = Console.ReadLine();
            } while (string.IsNullOrEmpty(path));

            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Directory not found. Try to input the path again.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            var options = optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=task1db;Trusted_Connection=True;").Options;
            UnitOfWork unitOfWork = new UnitOfWork(options);
            _fileImporter = new FileImporter(unitOfWork);
            _fileImporter.ImportFiles(path);
            Console.WriteLine("Import started...");
            do
            {
                Thread.Sleep(2000);
                Console.WriteLine(_fileImporter.GetInfo());
            } while(!_fileImporter.isFinished);
            Console.WriteLine("Import finished...");
        }
    }
}
