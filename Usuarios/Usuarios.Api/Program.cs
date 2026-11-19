using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Usuarios.Api.Middleware;
using Usuarios.Application.Behaviors;
using Usuarios.Application.Interfaces;
using Usuarios.Infrastructure.Clients;
using Usuarios.Infrastructure.Persistence;
using Usuarios.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Falta configurar ConnectionStrings:DefaultConnection.");
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Falta configurar Jwt:Key.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("Falta configurar Jwt:Issuer.");
var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("Falta configurar Jwt:Audience.");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("ConnectionStrings:DefaultConnection no puede estar vacía.");
}

if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException("Jwt:Key debe tener al menos 32 bytes.");
}

builder.Services.AddDbContext<UsuariosDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Usuarios.Application.Commands.RegistrarUsuario.RegistrarUsuarioCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Usuarios.Application.Commands.RegistrarUsuario.RegistrarUsuarioCommand).Assembly);

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITrabajoPortafolioRepository, TrabajoPortafolioRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRecuperacionContrasenaRepository, RecuperacionContrasenaRepository>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddSingleton<IPasswordResetSettings, PasswordResetSettings>();
builder.Services.AddScoped<IPerfilEstudianteRepository, PerfilEstudianteRepository>();
builder.Services.AddScoped<IPerfilClienteRepository, PerfilClienteRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<AdminInicialSeeder>();
builder.Services.AddScoped<IUsuarioAdminRepository, UsuarioAdminRepository>(); // ADMIN Marcos: HU-25/21/26/28

builder.Services.AddHttpClient<IPasswordStrengthChecker, HibpPasswordStrengthChecker>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(2.5);
    client.DefaultRequestHeaders.Add("User-Agent", "UsuariosApp-UTSJR");
});

builder.Services.AddTransient<JwtForwardingHandler>();
builder.Services.AddHttpClient<ICatalogoSkillsMarketplaceClient, CatalogoSkillsMarketplaceClient>(client =>
    {
        if (Uri.TryCreate(builder.Configuration["Servicios:MarketplaceUrl"], UriKind.Absolute, out var url))
            client.BaseAddress = url;
        client.Timeout = TimeSpan.FromSeconds(5);
    })
    .AddHttpMessageHandler<JwtForwardingHandler>();

// --- Autenticacion JWT ---
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
builder.Services.AddAuthorization();
// --- fin de autenticacion JWT ---


builder.Services.AddControllers();
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
app.UseAuthentication();   // este siempre va ANTES de UseAuthorization
app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" })).AllowAnonymous().ExcludeFromDescription();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UsuariosDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        await db.Database.MigrateAsync();
        logger.LogInformation("Migraciones aplicadas correctamente.");

        // Cuenta Administrador inicial (solo si AdminInicial:* está configurado y no existe)
        await scope.ServiceProvider.GetRequiredService<AdminInicialSeeder>().SembrarAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al aplicar migraciones de Entity Framework.");
    }
}

app.Run();
