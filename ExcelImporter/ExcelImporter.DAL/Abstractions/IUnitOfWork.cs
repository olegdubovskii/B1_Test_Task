
using ExcelImporter.DAL.Repositories;

namespace ExcelImporter.DAL.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        ExcelFileRepository ExcelFileRepository { get; }
        SheetRepository SheetRepository { get; }
        SheetClassRepository SheetClassRepository { get; }
        TotalBalanceAccountRepository TotalBalanceAccountRepository { get; }
        BalanceAccountRepository BalanceAccountRepository { get; }
        void Save();
    }
}
