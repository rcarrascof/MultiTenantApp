using MediatR;
using MultiTenantApp.Domain.Entities;

namespace MultiTenantApp.Application.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
        public string SlugTenant { get; set; }

        public GetProductByIdQuery(int id, string slugTenant)
        {
            Id = id;
            SlugTenant = slugTenant;
        }
    }
}
