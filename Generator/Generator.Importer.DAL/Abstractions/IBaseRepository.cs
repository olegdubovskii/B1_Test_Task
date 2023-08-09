
namespace Generator.Importer.DAL.Abstractions
{
    public interface IBaseRepository<T>
        where T : class
    {
        IEnumerable<T> GetItems();
        T GetItemByID(int id);
        void InsertItem(T item);
        void UpdateItem(T item);
        void DeleteItem(int id);
    }
}
