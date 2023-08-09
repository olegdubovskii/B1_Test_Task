using Generator.Importer.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Generator.Importer.DAL.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        FileStringRepository FileStringRepository { get; }
        void Save();
    }
}
