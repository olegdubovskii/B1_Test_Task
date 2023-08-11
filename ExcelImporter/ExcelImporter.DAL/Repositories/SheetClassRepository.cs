using ExcelImporter.DAL.Entities;

namespace ExcelImporter.DAL.Repositories
{
    public class SheetClassRepository : BaseRepository<SheetClass>
    {
        public SheetClassRepository(DatabaseContext context) : base(context) { }
    }
}
