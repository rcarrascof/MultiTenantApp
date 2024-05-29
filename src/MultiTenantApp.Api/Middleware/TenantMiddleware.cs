using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MultiTenantApp.API.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value.Split('/');
            if (path.Length > 1)
            {
                var tenant = path[1];
                context.Items["Tenant"] = tenant;
                var productsDbConnectionString = string.Format(
                    context.RequestServices.GetRequiredService<IConfiguration>().GetConnectionString("ProductsDb"),
                    tenant);
                context.Items["ProductsDb"] = productsDbConnectionString;
            }
            await _next(context);
        }
    }
}
