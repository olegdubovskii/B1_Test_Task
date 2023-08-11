using Generator.Importer.DAL.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Generator.Importer.DAL.Repositories
{
    /// <summary>
    /// Implementation of repository pattern
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private protected readonly DatabaseContext _context;
        private protected readonly DbSet<T> _dbSet;

        public BaseRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public virtual void DeleteItem(int id)
        {
            var item = GetItemByID(id);
            _dbSet.Remove(item);
        }

        public virtual T GetItemByID(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetItems()
        {
            return _dbSet.ToList();
        }

        public virtual void InsertItem(T item)
        {
            _dbSet.Add(item);
        }

        public virtual void UpdateItem(T item)
        {
            _dbSet.Update(item);
        }
    }
}
