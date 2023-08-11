using Generator.Importer.Abstractions;
using Generator.Importer.DAL;
using Generator.Importer.DAL.Entities;
using System.Diagnostics;

namespace Generator.Importer.Core
{
    public class FileImporter : IFileImporter
    {
        /// <summary>
        /// UOW object for working with the database
        /// </summary>
        private readonly UnitOfWork _unitOfWork;
        /// <summary>
        /// Count of strings that already have imported
        /// </summary>
        private int _importedStringsCount;
        /// <summary>
        /// Count of all strings to be imported
        /// </summary>
        private int _stringsForImportCount;
        /// <summary>
        /// Indicator for statistics timer 
        /// </summary>
        private bool _isFinished;
        public bool isFinished { get { return _isFinished; } }
        /// <summary>
        /// Timer for stats
        /// </summary>
        private Stopwatch _stopwatch;

        public FileImporter(UnitOfWork unitOfWork)
        {
            _isFinished = false;
            _stopwatch = new Stopwatch();
            _importedStringsCount = 0;
            _unitOfWork = unitOfWork;

        }
        /// <summary>
        /// Main import method. Creating new thread for strings import
        /// </summary>
        /// <param name="importDirectoryPath">Path to the directory where the files to be imported are located</param>
        public void ImportFiles(string importDirectoryPath)
        {
            var importThread = new Thread(() =>
            {
                FileInfo[] files = new DirectoryInfo(importDirectoryPath).GetFiles();
                //calculates how many strings need to be imported to display stats
                CalculateStringsForImportCount(files);
                _stopwatch.Restart();
                foreach (var file in files)
                {
                    ImportFile(file.FullName);
                }
                //closing context after import
                _unitOfWork.Dispose();
                _stopwatch.Stop();
                _isFinished = true;
            });
            importThread.IsBackground = true;
            importThread.Start();
        }

        /// <summary>
        /// Sends information with the percentage of imported strings
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            int percentComplete = (int)Math.Round((double)(100 * _importedStringsCount) / _stringsForImportCount);
            return $"Imported: {_importedStringsCount}|{_stringsForImportCount} strings. {percentComplete}% imported. Elapsed: {_stopwatch.Elapsed}";
        }

        /// <summary>
        /// Importing one file in all operation
        /// </summary>
        /// <param name="fileName">Name of file for import</param>
        private void ImportFile(string fileName)
        {
            int commitCount = 0;
            //span for string from importing file. Max lenght = 61 char because of our string standart
            ReadOnlySpan<char> readingSpan = stackalloc char[61];
            //stream reader to read all strings from file
            using (var reader = new StreamReader(fileName))
            {
                while (reader.Peek() != -1)
                {
                    readingSpan = reader.ReadLine().AsSpan();
                    //make commit to insert string to database
                    _unitOfWork.FileStringRepository.InsertItem(CreateEntity(ref readingSpan));
                    commitCount++;
                    //when 100000 rows are committed we need to save the changes and recreate the context to improve performance
                    if (commitCount == 100000)
                    {
                        commitCount = 0;
                        _unitOfWork.Save();
                        _importedStringsCount += 100000;
                        _unitOfWork.RecreateContext();

                    }
                }
                //saving for the case when a number not equal to 100000 is called in the loop
                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Creating an entity from string parts from a file
        /// </summary>
        /// <param name="entitySpan">String from file</param>
        /// <returns></returns>
        private FileString CreateEntity(ref ReadOnlySpan<char> entitySpan)
        {

            string date = new string(entitySpan.Slice(0, 10));
            string latinSymbols = new string(entitySpan.Slice(12, 10));
            string russianSymbols = new string(entitySpan.Slice(24, 10));

            int indexOfSlash = entitySpan.Slice(36, 10).IndexOf("|");
            int randomInt = int.Parse(entitySpan.Slice(36, indexOfSlash));

            int indexOfLastSlash = entitySpan.LastIndexOf("|");
            double randomDouble = double.Parse(entitySpan.Slice(36 + indexOfSlash + 2, indexOfLastSlash - 36 - indexOfSlash - 3));

            return new FileString(date, latinSymbols, russianSymbols, randomInt, randomDouble);
        }

        /// <summary>
        /// Calculating how many strings we need to import
        /// </summary>
        /// <param name="files">Files to import</param>
        private void CalculateStringsForImportCount(FileInfo[] files)
        {
            foreach(var file in files)
            {
                _stringsForImportCount += File.ReadLines(file.FullName).Count();
            }
        }
    }
}