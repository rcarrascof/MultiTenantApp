using MediatR;
using MultiTenantApp.Domain.Entities;

namespace MultiTenantApp.Application.Queries
{
    public class GetProductsListQuery : IRequest<List<Product>>
    {
        public string SlugTenant { get; set; }

        public GetProductsListQuery(string slugTenant)
        {
            SlugTenant = slugTenant;
        }
    }
}
