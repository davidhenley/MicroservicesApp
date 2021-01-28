using System.Threading.Tasks;
using Catalog.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);

            modelBuilder.Entity<Product>(builder =>
            {
                builder.Property(product => product.Price).HasPrecision(10, 2);
            });
        }
    }
}