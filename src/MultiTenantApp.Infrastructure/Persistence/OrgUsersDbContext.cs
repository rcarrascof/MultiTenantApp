using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Domain.Entities;

namespace MultiTenantApp.Infrastructure.Persistence
{
    public class OrgUsersDbContext : DbContext
    {
        public OrgUsersDbContext(DbContextOptions<OrgUsersDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
