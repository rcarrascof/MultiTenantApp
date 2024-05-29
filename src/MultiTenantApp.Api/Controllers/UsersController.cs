using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantApp.Application.Interfaces;
using System.Threading.Tasks;

namespace MultiTenantApp.API.Controllers
{
    [ApiController]
    [Route("api/{slugTenant}/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string slugTenant, int id)
        {
            var user = await _userService.GetUserByIdAsync(slugTenant, id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(user);
        }
    }
}
