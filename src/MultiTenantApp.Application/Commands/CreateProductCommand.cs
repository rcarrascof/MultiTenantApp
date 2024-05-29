using MediatR;

namespace MultiTenantApp.Application.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SlugTenant { get; set; }
    }
}
