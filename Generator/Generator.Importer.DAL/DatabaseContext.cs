using Generator.Importer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Generator.Importer.DAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<FileString> Strings { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
