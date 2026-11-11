using System.Text;
using FluentValidation;
using Marketplace.Api.Middleware;
using Marketplace.Application.Behaviors;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarOportunidades;
using Marketplace.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MarketplaceDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ListarOportunidadesQuery).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(ListarOportunidadesQuery).Assembly);

builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();

// --- Autenticación JWT (misma clave/issuer/audience que Usuarios.Api) ---
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero   // un token expirado debe dar 401 de inmediato
        };
    });
builder.Services.AddAuthorization();
// --- fin de autenticación JWT ---

// JSON camelCase (contrato del frontend)
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -- EMMA 23/09/2026: SE AGREGO LO DE CORS PARA QUE EL FRONT PUEDA CONSUMIR EL API
// CONFIG DE CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:8100",             // ESTE ES DESARROLLO LOCAL
                "http://localhost:4200",             // FRONTEND LOCAL
                "https://falta-dominio-frontend.com"    // AJUSTAMOS ESTE PA CUANDO YA ESTE ARRIBA 
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendPolicy");
app.UseAuthentication();   // siempre ANTES de UseAuthorization
app.UseAuthorization();

app.MapControllers();

// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<MarketplaceDbContext>();
        if (db.Database.IsRelational())
        {
            db.Database.Migrate();
            logger.LogInformation("Migraciones aplicadas correctamente.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al aplicar migraciones de Entity Framework.");
    }
}

app.Run();
