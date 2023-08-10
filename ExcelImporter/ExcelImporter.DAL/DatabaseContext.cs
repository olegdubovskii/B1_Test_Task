
using ExcelImporter.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExcelImporter.DAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ExcelFile> Files { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<SheetClass> SheetClasses { get; set; }
        public DbSet<TotalBalanceAccount> TotalBalanceAccounts { get; set; }
        public DbSet<BalanceAccount> BalanceAccounts { get; set; }
        public DatabaseContext()
        {
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
