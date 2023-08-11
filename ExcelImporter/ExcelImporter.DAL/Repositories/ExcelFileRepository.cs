using ExcelImporter.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExcelImporter.DAL.Repositories
{
    public class ExcelFileRepository : BaseRepository<ExcelFile> 
    {
        public ExcelFileRepository(DatabaseContext context) : base(context) { }
    }
}
