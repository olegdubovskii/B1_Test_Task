using ExcelImporter.DAL.Entities;

namespace ExcelImporter.DAL.Repositories
{
    public class SheetRepository : BaseRepository<Sheet>
    {   
        public SheetRepository(DatabaseContext context) : base(context) { }
    }
}
