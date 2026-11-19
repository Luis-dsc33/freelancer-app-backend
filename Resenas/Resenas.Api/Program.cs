using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using MediatR;
using Resenas.Application.Behaviors;
using Resenas.Application.Interfaces;
using Resenas.Api.Middleware;
using Resenas.Infrastructure.Clients;
using Resenas.Infrastructure.Persistence;
using Resenas.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Falta configurar ConnectionStrings:DefaultConnection.");
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Falta configurar Jwt:Key.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("Falta configurar Jwt:Issuer.");
var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("Falta configurar Jwt:Audience.");
if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
    throw new InvalidOperationException("Jwt:Key debe tener al menos 32 bytes.");

builder.Services.AddDbContext<ResenasDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IResenaRepository, ResenaRepository>();
builder.Services.AddTransient<JwtForwardingHandler>();

builder.Services.AddHttpClient<IMarketplaceContratosClient, MarketplaceContratosClient>(http =>
    {
        if (Uri.TryCreate(builder.Configuration["Servicios:MarketplaceUrl"], UriKind.Absolute, out var url))
            http.BaseAddress = url;
        http.Timeout = TimeSpan.FromSeconds(5);
    })
    .AddHttpMessageHandler<JwtForwardingHandler>();

builder.Services.AddHttpClient<IUsuariosPublicoClient, UsuariosPublicoClient>(http =>
    {
        if (Uri.TryCreate(builder.Configuration["Servicios:UsuariosUrl"], UriKind.Absolute, out var url))
            http.BaseAddress = url;
        http.Timeout = TimeSpan.FromSeconds(3);
    })
    .AddHttpMessageHandler<JwtForwardingHandler>();

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddControllers();
var applicationAssembly = typeof(ValidationBehavior<,>).Assembly;
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(applicationAssembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(applicationAssembly);
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var origenesPermitidos = new HashSet<string>(
    [
        "http://localhost:8100",
        "http://localhost:4200",
        "http://localhost",
        "https://localhost",
        "capacitor://localhost",
        "ionic://localhost"
    ],
    StringComparer.OrdinalIgnoreCase);

var origenesConfigurados = new List<string>();
var origenesCsv = builder.Configuration["Cors:AllowedOrigins"];
if (!string.IsNullOrWhiteSpace(origenesCsv))
    origenesConfigurados.AddRange(origenesCsv.Split([',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

var origenesArray = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
if (origenesArray is { Length: > 0 })
    origenesConfigurados.AddRange(origenesArray);

foreach (var origen in origenesConfigurados)
{
    var normalizado = origen.Trim().TrimEnd('/');
    if (!string.IsNullOrWhiteSpace(normalizado))
        origenesPermitidos.Add(normalizado);
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins([.. origenesPermitidos])
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseForwardedHeaders();

if (app.Environment.IsProduction() && origenesConfigurados.Count == 0)
{
    app.Logger.LogWarning("Cors:AllowedOrigins no está configurado; configura CORS_ALLOWED_ORIGINS en .env con el dominio HTTPS del frontend.");
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" })).AllowAnonymous().ExcludeFromDescription();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ResenasDbContext>();
        if (db.Database.IsRelational())
        {
            db.Database.Migrate();
            logger.LogInformation("Migraciones de Resenas aplicadas correctamente.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al aplicar migraciones de Entity Framework en Resenas.");
    }
}

app.Run();
