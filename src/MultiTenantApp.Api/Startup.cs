using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MultiTenantApp.API.Middleware;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using MultiTenantApp.Infrastructure.Persistence;
using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Application.Services;
using MultiTenantApp.Application.Commands;
using MultiTenantApp.Application.Handlers;
using MultiTenantApp.Application.Queries;
using MultiTenantApp.Domain.Entities;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

        services.AddControllers();

        services.AddDbContext<OrgUsersDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("OrgUsersDb")));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<OrganizationService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Startup).Assembly));
        services.AddScoped<IRequestHandler<CreateProductCommand, int>, ProductCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateProductCommand, Unit>, ProductCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteProductCommand, Unit>, ProductCommandHandler>();
        services.AddScoped<IRequestHandler<GetProductByIdQuery, Product>, ProductQueryHandler>();
        services.AddScoped<IRequestHandler<GetProductsListQuery, List<Product>>, ProductQueryHandler>();

        services.AddScoped<ProductsDbContextFactory>();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MultiTenantApp", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<TenantMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MultiTenantApp V1");
            c.RoutePrefix = "swagger";
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }



}
