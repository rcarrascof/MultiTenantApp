using MediatR;

namespace MultiTenantApp.Application.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string SlugTenant { get; set; }
    }
}
