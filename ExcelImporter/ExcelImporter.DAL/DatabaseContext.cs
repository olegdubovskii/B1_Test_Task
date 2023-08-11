
using ExcelImporter.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExcelImporter.DAL
{
    /// <summary>
    /// Entity framework class for database access
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<ExcelFile> Files { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<SheetClass> SheetClasses { get; set; }
        public DbSet<TotalBalanceAccount> TotalBalanceAccounts { get; set; }
        public DbSet<BalanceAccount> BalanceAccounts { get; set; }
        public DatabaseContext()
        {
            //ensures that the database for the context exists 
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //configuration for getting connection string to the database
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json").Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder
                //lazy loading is the process whereby an entity or collection of entities is automatically loaded
                //from the database the first time
                //that a property referring to the entity/entities is accessed.
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString);
        }
    }
}
