using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MultiTenantApp.Infrastructure.Persistence
{
    public class ProductsDbContextFactory : IDesignTimeDbContextFactory<ProductsDbContext>
    {
        public ProductsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ProductsDbContext>();
            var tenant = args.Length > 0 ? args[0] : "default";
            var connectionString = string.Format(configuration.GetConnectionString("ProductsDb"), tenant);

            optionsBuilder.UseSqlServer(connectionString);

            return new ProductsDbContext(optionsBuilder.Options, tenant);
        }

        public ProductsDbContext CreateDbContext(string tenant)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductsDbContext>();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = string.Format(configuration.GetConnectionString("ProductsDb"), tenant);
            optionsBuilder.UseSqlServer(connectionString);

            return new ProductsDbContext(optionsBuilder.Options, tenant);
        }
    }
}
