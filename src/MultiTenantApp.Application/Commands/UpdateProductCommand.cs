using MediatR;

namespace MultiTenantApp.Application.Commands
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SlugTenant { get; set; }
    }
}
