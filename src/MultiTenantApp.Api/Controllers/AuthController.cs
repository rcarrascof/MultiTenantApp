﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MultiTenantApp.API.Models;
using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);  
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim("tenant", user.Organization.Slug),
                    new Claim("organization", user.Organization.Name)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    organizationId = user.OrganizationId,
                    organizationName = user.Organization.Name
                }
            });
        }
    }
}
