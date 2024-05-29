using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Domain.Entities;
using MultiTenantApp.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiTenantApp.Application.Handlers
{
    public class ProductQueryHandler :
        IRequestHandler<GetProductByIdQuery, Product>,
        IRequestHandler<GetProductsListQuery, List<Product>>
    {
        private readonly ProductsDbContextFactory _contextFactory;

        public ProductQueryHandler(ProductsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext(request.SlugTenant);
            return await context.Products.FindAsync(request.Id);
        }

        public async Task<List<Product>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext(request.SlugTenant);
            return await context.Products.ToListAsync(cancellationToken);
        }
    }
}
