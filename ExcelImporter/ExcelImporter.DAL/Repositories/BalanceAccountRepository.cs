using ExcelImporter.DAL.Entities;

namespace ExcelImporter.DAL.Repositories
{
    public class BalanceAccountRepository : BaseRepository<BalanceAccount>
    {
        public BalanceAccountRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
