using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Usuarios.Api.Middleware;
using Usuarios.Application.Behaviors;
using Usuarios.Application.Interfaces;
using Usuarios.Infrastructure.Persistence;
using Usuarios.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<UsuariosDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Usuarios.Application.Commands.RegistrarUsuario.RegistrarUsuarioCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Usuarios.Application.Commands.RegistrarUsuario.RegistrarUsuarioCommand).Assembly);

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();

// --- Autenticación JWT ---
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization();
// --- fin de autenticación JWT ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();   // este siempre va ANTES de UseAuthorization
app.UseAuthorization();
app.MapControllers();

// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<UsuariosDbContext>();
        db.Database.Migrate();
        logger.LogInformation("Migraciones aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al aplicar migraciones de Entity Framework.");
    }
}

app.Run();