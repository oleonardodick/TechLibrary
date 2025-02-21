using Microsoft.EntityFrameworkCore;
using TechLibrary.Api.Domain.Entities;

namespace TechLibrary.Api.Infrastructure.DataAccess
{
    public class TechLibraryDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var solutionPath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName;
            var dbPath = Path.Combine(solutionPath ?? "", "TechLibrary\\TechLibraryDb.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

        }
    }
}
