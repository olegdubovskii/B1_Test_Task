using Generator.Joiner.Abstractions;

namespace Generator.Joiner.Core
{
    public class FileJoiner : IFileJoiner
    {
        private static object _locker = new object();
        private int _joinedFilesCount;
        private int _deletedStringsCount;

        public int JoinFiles(string joinDirectoryPath, string deleteString)
        {
            _joinedFilesCount = 0;
            _deletedStringsCount = 0;
            var sw = new StreamWriter($"{joinDirectoryPath}\\joined\\joined.txt");
            string[] fileNames = Directory.GetFiles(joinDirectoryPath);
            foreach (var fileName in fileNames)
            {
                ThreadPool.QueueUserWorkItem((p) => JoinFile(sw, fileName, deleteString, fileNames.Length));
            }
            lock (_locker)
            {
                Monitor.Wait(_locker);
            }
            sw.Close();
            return _deletedStringsCount;
        }

        private void JoinFile(StreamWriter sw, string fileName, string deleteString, int filesInDirectoryCount)
        {
            ReadOnlySpan<char> readingSpan = stackalloc char[61];
            using (var reader = new StreamReader(fileName))
            {
                while(reader.Peek() != -1)
                {
                    readingSpan = reader.ReadLine().AsSpan();
                    if (!string.IsNullOrEmpty(deleteString))
                    {
                        if (!MemoryExtensions.Contains(readingSpan, deleteString.AsSpan(), StringComparison.Ordinal))
                        {
                            lock (_locker)
                            {
                                sw.WriteLine(readingSpan);
                                sw.Flush();
                            }
                        }
                        else
                        {
                            Interlocked.Increment(ref _deletedStringsCount);
                        }
                    } else
                    {
                        lock (_locker)
                        {
                            sw.WriteLine(readingSpan);
                            sw.Flush();
                        }
                    }
                }
            }
            Interlocked.Increment(ref _joinedFilesCount);
            if (_joinedFilesCount == filesInDirectoryCount)
            {
                lock(_locker)
                {
                    Monitor.Pulse(_locker);
                }
            }
        }
    }
}
