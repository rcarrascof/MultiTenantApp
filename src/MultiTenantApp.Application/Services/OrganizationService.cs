using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MultiTenantApp.Domain.Entities;
using MultiTenantApp.Infrastructure.Persistence;
using System.Threading.Tasks;

namespace MultiTenantApp.Application.Services
{
    public class OrganizationService
    {
        private readonly OrgUsersDbContext _orgUsersDbContext;
        private readonly ProductsDbContextFactory _productsDbContextFactory;

        public OrganizationService(OrgUsersDbContext orgUsersDbContext, ProductsDbContextFactory productsDbContextFactory)
        {
            _orgUsersDbContext = orgUsersDbContext;
            _productsDbContextFactory = productsDbContextFactory;
        }

        public async Task<Organization> CreateOrganizationAsync(string organizationName, string slug, string username, string password)
        {
            var organization = new Organization
            {
                Name = organizationName,
                Slug = slug
            };

            _orgUsersDbContext.Organizations.Add(organization);
            await _orgUsersDbContext.SaveChangesAsync();

            var user = new User
            {
                Username = username,
                Password = password,
                OrganizationId = organization.Id
            };

            _orgUsersDbContext.Users.Add(user);
            await _orgUsersDbContext.SaveChangesAsync();

            await CreateProductsDatabaseAsync(slug);

            return organization;
        }

        private async Task CreateProductsDatabaseAsync(string slugTenant)
        {
            using var context = _productsDbContextFactory.CreateDbContext(slugTenant);
            await context.Database.MigrateAsync();
        }
    }
}
