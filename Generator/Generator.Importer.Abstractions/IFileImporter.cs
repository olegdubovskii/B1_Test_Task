
namespace Generator.Importer.Abstractions
{
    public interface IFileImporter
    {
        public void ImportFiles(string importDirectoryPath);
        public string GetInfo();
    }
}
