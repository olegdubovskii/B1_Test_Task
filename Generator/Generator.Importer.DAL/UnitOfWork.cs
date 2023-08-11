using Generator.Importer.DAL.Abstractions;
using Generator.Importer.DAL.Repositories;

namespace Generator.Importer.DAL
{
    /// <summary>
    /// Implementation of Unit Of Work pattern for easy work with several repositories and to be sure that we use one database context
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private DatabaseContext _databaseContext;
        private FileStringRepository _fileStringRepository;

        public UnitOfWork()
        {
            _databaseContext = new DatabaseContext();
        }

        /// <summary>
        /// FileStringRepository getter
        /// </summary>
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

        /// <summary>
        /// Saving all commited changes in the database
        /// </summary>
        public void Save()
        {
            _databaseContext.SaveChanges();
        }

        /// <summary>
        /// Disposing context
        /// </summary>
        public void Dispose()
        {
            _databaseContext.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Recreating context. We need to invoke it when we have already done a lot of database accesses
        /// </summary>
        public void RecreateContext()
        {
            Dispose();
            _databaseContext = new DatabaseContext();
            _fileStringRepository = new FileStringRepository(_databaseContext);

        }
    }
}
