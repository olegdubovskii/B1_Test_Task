using Generator.Importer.DAL.Abstractions;
using Generator.Importer.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Generator.Importer.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private DatabaseContext _databaseContext;
        private FileStringRepository _fileStringRepository;
        private bool _disposed = false;

        public UnitOfWork(DbContextOptions<DatabaseContext> options)
        {
            _databaseContext = new DatabaseContext(options);
        }

        public FileStringRepository FileStringRepository
        {
            get
            {

                if (_fileStringRepository is null)
                {
                    _fileStringRepository = new FileStringRepository(_databaseContext);
                }
                return _fileStringRepository;
            }
        }

        public void Save()
        {
            _databaseContext.SaveChanges();
        }

        public void Dispose()
        {
            _databaseContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public void RecreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            var options = optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=task1db;Trusted_Connection=True;").Options;
            _databaseContext.Dispose();
            _databaseContext = new DatabaseContext(options);
            _fileStringRepository = new FileStringRepository(_databaseContext);

        }
    }
}
