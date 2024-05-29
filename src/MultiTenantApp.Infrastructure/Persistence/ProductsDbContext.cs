using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Domain.Entities;

namespace MultiTenantApp.Infrastructure.Persistence
{
    public class ProductsDbContext : DbContext
    {
        public string Tenant { get; }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options, string tenant = null)
            : base(options)
        {
            Tenant = tenant;
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(Tenant))
            {
                var connectionString = string.Format(
                    "Server=RCARRASCO-LAP\\MSSQLSERVER02;Database={0}_ProductsDb;User Id=sa;Password=wladimir;TrustServerCertificate=True;",
                    Tenant
                );
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
