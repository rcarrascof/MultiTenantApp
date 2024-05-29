using Microsoft.AspNetCore.Mvc;
using MultiTenantApp.API.Models;
using MultiTenantApp.Application.Services;
using System.Threading.Tasks;

namespace MultiTenantApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationsController : ControllerBase
    {
        private readonly OrganizationService _organizationService;

        public OrganizationsController(OrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequest request)
        {
            var organization = await _organizationService.CreateOrganizationAsync(
                request.OrganizationName, request.Slug, request.Username, request.Password);

            if (organization == null)
            {
                return BadRequest(new { message = "Failed to create organization" });
            }

            return Ok(new { message = "Organization created successfully", organization });
        }
    }
}
