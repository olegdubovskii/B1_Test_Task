using Generator.Importer.Abstractions;
using Generator.Importer.DAL;
using Generator.Importer.DAL.Entities;
using System.Diagnostics;

namespace Generator.Importer.Core
{
    public class FileImporter : IFileImporter
    {
        private readonly UnitOfWork _unitOfWork;
        private int _importedStringsCount;
        private int _stringsForImportCount;
        private bool _isFinished;
        public bool isFinished { get { return _isFinished; } }
        private Stopwatch _stopwatch;

        public FileImporter(UnitOfWork unitOfWork)
        {
            _isFinished = false;
            _stopwatch = new Stopwatch();
            _importedStringsCount = 0;
            _unitOfWork = unitOfWork;

        }
        public void ImportFiles(string importDirectoryPath)
        {
            var importThread = new Thread(() =>
            {
                FileInfo[] files = new DirectoryInfo(importDirectoryPath).GetFiles();
                CalculateStringsForImportCount(files);
                _stopwatch.Restart();
                foreach (var file in files)
                {
                    ImportFile(file.FullName);
                }
                _stopwatch.Stop();
                _isFinished = true;
            });
            importThread.IsBackground = true;
            importThread.Start();
        }

        public string GetInfo()
        {
            int percentComplete = (int)Math.Round((double)(100 * _importedStringsCount) / _stringsForImportCount);
            return $"Imported: {_importedStringsCount}|{_stringsForImportCount} strings. {percentComplete}% imported. Elapsed: {_stopwatch.Elapsed}";
        }

        private void ImportFile(string fileName)
        {
            int commitCount = 0;
            ReadOnlySpan<char> readingSpan = stackalloc char[61];
            using (var reader = new StreamReader(fileName))
            {
                while (reader.Peek() != -1)
                {
                    readingSpan = reader.ReadLine().AsSpan();
                    _unitOfWork.FileStringRepository.InsertItem(CreateEntity(ref readingSpan));
                    commitCount++;
                    if (commitCount == 100000)
                    {
                        commitCount = 0;
                        _unitOfWork.Save();
                        _importedStringsCount += 100000;
                        _unitOfWork.RecreateContext();

                    }
                }
                _unitOfWork.Save();
            }
        }

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

        private void CalculateStringsForImportCount(FileInfo[] files)
        {
            foreach(var file in files)
            {
                _stringsForImportCount += File.ReadLines(file.FullName).Count();
            }
        }
    }
}