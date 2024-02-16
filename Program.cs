
using System.Security.Claims;
using System.Text;
using backend.Authentication;
using backend.Authentication.RoleAccess;
using backend.Models;
using backend.Repositories;
using backend.Repositories.Interfaces;
using backend.Services;
using backend.Services.Interfaces;
using backend.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace backend;

public static class Program
{
    public static T GetFromConfig<T>(this WebApplicationBuilder builder, string configSection)
    {
        return builder.Configuration.GetSection(configSection).Get<T>()
               ?? throw new Exception($"No se pudo encontrar el valor {configSection} en la configuracion");
    }

    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Only allow things within this namespace to log to file
        builder.Logging.AddFilter<TextLoggerProvider>((category, _) => category?.StartsWith("backend") ?? false);
        builder.Logging.AddProvider(new TextLoggerProvider("../log.txt"));

        // Add services to the container.
        builder.Services.AddAuthorization(options =>
        {
            foreach (Access access in Enum.GetValues<Access>())
            {
                options.AddPolicy(access.ToString(), policy =>
                {
                    policy.Requirements.Add(new AccessRequirement(access));
                });
            }
        });

        // Right now it doesn't hold state, so it should be recycled.
        builder.Services.AddSingleton<IAuthorizationHandler, AccessHandler>();

        string jwtIssuer = builder.GetFromConfig<string>("Jwt:Issuer");
        string jwtKey = builder.GetFromConfig<string>("Jwt:Key");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                // Who makes
                ValidIssuer = jwtIssuer,
                // Who receves
                ValidAudience = jwtIssuer, // This is so comical
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            };
        });

        string rootDirectory = Path.Combine("..", "res");
        string usersJsonFile = Path.Combine(rootDirectory,
            builder.GetFromConfig<string>("JsonFile:Users"));
        string clientsJsonFile = Path.Combine(rootDirectory,
            builder.GetFromConfig<string>("JsonFile:Clients"));
        string productStatusJsonFile = Path.Combine(rootDirectory,
            builder.GetFromConfig<string>("JsonFile:ProductStatuses")
        );

        JsonClientRepository clientRepo = new(clientsJsonFile, 20);

        builder.Services
        .AddSingleton<Dictionary<string, IRoleAccess>>(_ => new()
        {
            {AgenteServiciosRoleAccess.Instance.RoleName, AgenteServiciosRoleAccess.Instance},
            {GerenteRoleAccess.Instance.RoleName, GerenteRoleAccess.Instance},
        }).AddSingleton<IRoleAccessFactory, DefaultRoleAccessFactory>()
        .AddSingleton<IUserRepository, JsonUserRepository>(_ => new(usersJsonFile))
        .AddScoped<IUserService, DefaultUserService>()
         // Theses are going to break so hard when trying to access concurrently to the same file.
        .AddSingleton<IClientRepository, JsonClientRepository>(_ => clientRepo)
        .AddScoped<IClientService, DefaultClientService>()
        .AddSingleton<IProductNameCheck, JsonClientRepository>(_ => clientRepo)
        .AddSingleton<IProductRepository, JsonClientRepository>(_ => clientRepo)
        .AddSingleton<INameProduct, DefaultProductService>()
        .AddSingleton<IProductFactory, ProductoFactory>()
        .AddSingleton<IProductStatusRepository, JsonProductStatusRepository>(_ => new(
            productStatusJsonFile,
            new FilterByRoleProductStatus()
        ))
        .AddScoped<IProductService, DefaultProductService>()
        .AddSingleton<ISeedProductStatusRepository, JsonProductStatusRepository>(_ => new(
            productStatusJsonFile,
            new FilterByRoleProductStatus()
        ))
        .AddScoped<IProductStatusService, DefaultProductService>();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        // From https://www.codeindotnet.com/jwt-bearer-token-authorization-in-swagger-api/
        builder.Services.AddSwaggerGen(swagger =>
        {
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                      new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                }
            });
        });

        var app = builder.Build();

        app.Services.GetService<ISeedProductStatusRepository>()?.SeedFile(productStatusJsonFile);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(options =>
        {
            // Allow frontend address api calls
            options.WithOrigins("http://localhost:5000").AllowAnyMethod().AllowAnyHeader();
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
