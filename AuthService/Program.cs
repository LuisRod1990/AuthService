using AuthService.Application.UseCases;
using AuthService.Domain.Ports;
using AuthService.Domain.Repositories;
using AuthService.Domain.Services;
using AuthService.Infrastructure.Adapters;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("CONN")
    ?? "Defaults";
Console.WriteLine($"Connection String: {connectionString}");
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "DefaultKey";
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "DefaultIssuer";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "DefaultAudience";
var corsName = Environment.GetEnvironmentVariable("CORS_NAME") ?? "DefaultCors";
var corsHost = Environment.GetEnvironmentVariable("CORS_HOST") ?? "*";
Console.WriteLine($"jwtKey: {jwtKey}");
Console.WriteLine($"jwtIssuer: {jwtIssuer}");
Console.WriteLine($"jwtAudience: {jwtAudience}");
Console.WriteLine($"corsName: {corsName}");
Console.WriteLine($"corsHost: {corsHost}");

// Configuración detallada del DbContext para logging
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString)
           .EnableSensitiveDataLogging()   // muestra la cadena de conexión usada. Eliminar en producción
           .LogTo(Console.WriteLine, LogLevel.Information)); // log detallado. Eliminar en producción

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsName, policy =>
    {
        policy.WithOrigins(corsHost)
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Security API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Introduce el token JWT con el formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUsuarioSeguridadRepository, UsuarioSeguridadRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IPermisosRepository, PermisosRepository>();
builder.Services.AddScoped<IRegisterUser, RegisterUser>();
builder.Services.AddScoped<ILoginUser, LoginUser>();
builder.Services.AddScoped<IUpdateUserPassword, UpdateUserPassword>();

builder.Services.AddSingleton<IDateTimeProvider, MexicoDateTimeProvider>();

builder.Services.AddScoped<PasswordHasherService>();
builder.Services.AddScoped<ITokenService, JwtService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AuthDbContext>();
        context.Database.OpenConnection();
        Console.WriteLine("✅ Conexión exitosa a Cloud SQL Server");
        context.Database.CloseConnection();
    }
    catch (SqlException ex)
    {
        Console.WriteLine("❌ SQL ERROR DETALLADO:");
        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine($"Error Number: {ex.Number}");
        Console.WriteLine($"Error State: {ex.State}");
        Console.WriteLine($"Error Class: {ex.Class}");
        Console.WriteLine($"Server: {ex.Server}");
        Console.WriteLine($"Source: {ex.Source}");
        Console.WriteLine($"Procedure: {ex.Procedure}");
        Console.WriteLine($"LineNumber: {ex.LineNumber}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
        Console.WriteLine($"ClientConnectionId: {ex.ClientConnectionId}");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR GENERAL DETALLADO:");
        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"InnerException: {ex.InnerException.Message}");
        }
    }
}


var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Cadena de conexión: {conn}", connectionString);

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(corsName);


app.MapControllers();

app.Run();