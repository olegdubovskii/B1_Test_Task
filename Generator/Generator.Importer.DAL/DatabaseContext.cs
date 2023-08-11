using Generator.Importer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Generator.Importer.DAL
{
    /// <summary>
    /// Entity framework class for database access
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<FileString> FileString { get; set; }

        public DatabaseContext()
        {
            //ensures that the database for the context exists 
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json").Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
