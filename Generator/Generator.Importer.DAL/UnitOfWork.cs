using Generator.Importer.DAL.Abstractions;
using Generator.Importer.DAL.Repositories;

namespace Generator.Importer.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private DatabaseContext _databaseContext;
        private FileStringRepository _fileStringRepository;

        public UnitOfWork()
        {
            _databaseContext = new DatabaseContext();
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
            Dispose();
            _databaseContext = new DatabaseContext();
            _fileStringRepository = new FileStringRepository(_databaseContext);

        }
    }
}
