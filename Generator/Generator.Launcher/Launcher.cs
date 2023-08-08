using Generator.Core;
using Generator.Joiner.Core;
using System.Diagnostics;

namespace Generator.Launcher
{
    public class Launcher
    {
        private static readonly Stopwatch _stopwatch = new Stopwatch();
        private static FileGenerator? _fileGenerator;
        private static FileJoiner? _fileJoiner;
        public static void LaunchApplication()
        {
            int commandNumber = 0;
            while (commandNumber != 3)
            {
                Console.WriteLine("Enter the command number:\n" +
                    "1.Generate files\n" +
                    "2.Join files");
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
                            commandNumber = 3;
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
            _stopwatch.Reset();
            _stopwatch.Start();
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
            _stopwatch.Reset();
            _stopwatch.Start();
            int deletedStringsCount = _fileJoiner.JoinFiles(path, deleteString);
            _stopwatch.Stop();
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                _stopwatch.Elapsed.Hours,
                _stopwatch.Elapsed.Minutes,
                _stopwatch.Elapsed.Seconds,
                _stopwatch.Elapsed.Milliseconds / 10);
            Console.WriteLine($"Files joined. Deleted strings: {deletedStringsCount}. Elapsed time: {elapsedTime}.");
        }
    }
}
