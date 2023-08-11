using ExcelImporter.DAL.Entities;

namespace ExcelImporter.DAL.Repositories
{
    public class TotalBalanceAccountRepository : BaseRepository<TotalBalanceAccount>
    {
        public TotalBalanceAccountRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
