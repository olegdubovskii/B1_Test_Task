using Generator.Importer.DAL.Entities;

namespace Generator.Importer.DAL.Repositories
{
    public class FileStringRepository : BaseRepository<FileString>
    {
        public FileStringRepository(DatabaseContext context) : base(context) { }
    }
}
