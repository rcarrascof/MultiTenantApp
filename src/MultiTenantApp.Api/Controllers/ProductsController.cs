using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantApp.Application.Commands;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiTenantApp.API.Controllers
{
    [ApiController]
    [Route("api/{slugTenant}/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(string slugTenant, int id)
        {
            var query = new GetProductByIdQuery(id, slugTenant);
            var product = await _mediator.Send(query);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts(string slugTenant)
        {
            var query = new GetProductsListQuery(slugTenant);
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateProduct(string slugTenant, [FromBody] CreateProductCommand command)
        {
            command.SlugTenant = slugTenant;
            var productId = await _mediator.Send(command);
            return Ok(productId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string slugTenant, int id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { message = "Product ID mismatch" });
            }
            command.SlugTenant = slugTenant;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string slugTenant, int id)
        {
            var command = new DeleteProductCommand { Id = id, SlugTenant = slugTenant };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
