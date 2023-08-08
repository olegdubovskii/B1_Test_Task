namespace Generator.Joiner.Abstractions
{
    public interface IFileJoiner
    {
        public int JoinFiles(string joinDirectoryPath, string deleteString);
    }
}
