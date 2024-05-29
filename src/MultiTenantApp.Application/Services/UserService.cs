using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Domain.Entities;
using MultiTenantApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly OrgUsersDbContext _context;

        public UserService(OrgUsersDbContext context)
        {
            _context = context;
        }


        public async Task<User> Authenticate(string username, string password)
        {
            // Lógica de autenticación
            var user = await _context.Users
                .Include(u => u.Organization)
                .SingleOrDefaultAsync(u => u.Username == username && u.Password == password);

            return user;
        }

        public async Task<User> GetUserByIdAsync(string slugTenant, int userId)
        {
            var user = await _context.Users
                .Include(u => u.Organization)
                .FirstOrDefaultAsync(u => u.Id == userId && u.Organization.Slug == slugTenant);

            return user;
        }
    }

}
