using Generator.Joiner.Abstractions;

namespace Generator.Joiner.Core
{
    /// <summary>
    /// Merging files within a specified directory
    /// </summary>
    public class FileJoiner : IFileJoiner
    {
        /// <summary>
        /// Lock object for threads synchronization
        /// </summary>
        private static object _locker = new object();
        /// <summary>
        /// Count of joined files per all join operation
        /// </summary>
        private int _joinedFilesCount;
        /// <summary>
        /// Count of deleted strings, which equals to user delete string, per all operation 
        /// </summary>
        private int _deletedStringsCount;

        /// <summary>
        /// Main join method. Creating tasks for thread pool which joins one file
        /// </summary>
        /// <param name="joinDirectoryPath">Directory where files for join are locating</param>
        /// <param name="deleteString">Substring to remove strings from merged files that contain it</param>
        /// <returns>Count of deleted strings during the join</returns>
        public int JoinFiles(string joinDirectoryPath, string deleteString)
        {
            _joinedFilesCount = 0;
            _deletedStringsCount = 0;
            //streamwriter for outcoming file
            var sw = new StreamWriter($"{joinDirectoryPath}\\joined\\joined.txt");
            string[] fileNames = Directory.GetFiles(joinDirectoryPath);
            foreach (var fileName in fileNames)
            {
                ThreadPool.QueueUserWorkItem((p) => JoinFile(sw, fileName, deleteString, fileNames.Length));
            }
            //lock for main thread
            lock (_locker)
            {
                //thw main thread waiting in 'waiting queue' until last thread from nested method will send pulse to it
                Monitor.Wait(_locker);
            }
            sw.Close();
            return _deletedStringsCount;
        }

        /// <summary>
        /// Reading strings from file, deleting strings that have a 'deleteString' substring, writing filtered strings to joined file
        /// </summary>
        /// <param name="sw">Stream writer for writing to new joined file</param>
        /// <param name="fileName">Merge file name</param>
        /// <param name="deleteString">Substring for deleting strings from merge file</param>
        /// <param name="filesInDirectoryCount">Count of all files in directory</param>
        private void JoinFile(StreamWriter sw, string fileName, string deleteString, int filesInDirectoryCount)
        {
            //span for string from merge file. Max lenght can be 61 char
            ReadOnlySpan<char> readingSpan = stackalloc char[61];
            using (var reader = new StreamReader(fileName))
            {
                while (reader.Peek() != -1)
                {
                    //read string from merged file to span
                    readingSpan = reader.ReadLine().AsSpan();
                    //if the delete string is empty, we join all strings from file
                    if (!string.IsNullOrEmpty(deleteString))
                    {
                        //method which checks is temporary string(span) contains delete string
                        if (!MemoryExtensions.Contains(readingSpan, deleteString.AsSpan(), StringComparison.Ordinal))
                        {
                            //lock for writing to file
                            lock (_locker)
                            {
                                sw.WriteLine(readingSpan);
                                sw.Flush();
                            }
                        }
                        else
                        {
                            //atomic increase of deleted strings count for all threads
                            Interlocked.Increment(ref _deletedStringsCount);
                        }
                    }
                    else
                    {
                        //lock for writing to file
                        lock (_locker)
                        {
                            sw.WriteLine(readingSpan);
                            sw.Flush();
                        }
                    }
                }
            }
            //atomic increase of joined strings count for all threads
            Interlocked.Increment(ref _joinedFilesCount);
            //if we joined all strings we pulse the main thread from this lock
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
