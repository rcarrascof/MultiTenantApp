using MediatR;
using MultiTenantApp.Application.Commands;
using MultiTenantApp.Domain.Entities;
using MultiTenantApp.Infrastructure.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiTenantApp.Application.Handlers
{
    public class ProductCommandHandler :
        IRequestHandler<CreateProductCommand, int>,
        IRequestHandler<UpdateProductCommand, Unit>,
        IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly ProductsDbContextFactory _contextFactory;

        public ProductCommandHandler(ProductsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext(new[] { request.SlugTenant });

                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price
                };

                context.Products.Add(product);
                await context.SaveChangesAsync(cancellationToken);

                return product.Id;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                throw new ApplicationException($"Error creating product for tenant {request.SlugTenant}", ex);
            }
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext(new[] { request.SlugTenant });

                var product = await context.Products.FindAsync(request.Id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {request.Id} not found");
                }

                product.Name = request.Name;
                product.Price = request.Price;

                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                throw new ApplicationException($"Error updating product for tenant {request.SlugTenant}", ex);
            }
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext(new[] { request.SlugTenant });

                var product = await context.Products.FindAsync(request.Id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {request.Id} not found");
                }

                context.Products.Remove(product);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                throw new ApplicationException($"Error deleting product for tenant {request.SlugTenant}", ex);
            }
        }
    }
}
