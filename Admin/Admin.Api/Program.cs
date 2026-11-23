using Microsoft.Extensions.DependencyInjection.Extensions; // ADMIN Marcos: TryAddTransient
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using MediatR;
using Admin.Application.Behaviors;
using Admin.Api.Middleware;
using Admin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Admin.Application.Interfaces;
using Admin.Application.Queries.ObtenerMetricas;
using Admin.Infrastructure.Clients;
using Admin.Infrastructure.Fakes;
using Admin.Infrastructure.Settings;
using Microsoft.OpenApi;

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

// --- MediatR + validaciones (CQRS) ---
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ObtenerMetricasQuery).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(ObtenerMetricasQuery).Assembly);

// --- HU-28: fuentes de estadísticas ---
// Admin no consulta schemas ajenos: pide las estadísticas por HTTP a cada servicio dueño.
// Mientras esos endpoints no existan, Metricas:UsarDatosDemostracion = true usa los clientes Fake.
builder.Services.AddSingleton<IMetricasSettings, MetricasSettings>();
builder.Services.AddHttpContextAccessor();

if (builder.Configuration.GetValue<bool>("Metricas:UsarDatosDemostracion"))
{
    builder.Services.AddScoped<IUsuariosClient, UsuariosFakeClient>();
    builder.Services.AddScoped<IMarketplaceClient, MarketplaceFakeClient>();
    builder.Services.AddScoped<IResenasClient, ResenasFakeClient>();
}
else
{
    builder.Services.AddTransient<ReenviarTokenHandler>();

    void Registrar<TInterfaz, TImplementacion>(string claveUrl)
        where TInterfaz : class where TImplementacion : class, TInterfaz =>
        builder.Services.AddHttpClient<TInterfaz, TImplementacion>(http =>
            {
                // URL vacía o inválida = el servicio queda "No disponible" en el panel, sin tumbar la petición
                if (Uri.TryCreate(builder.Configuration[claveUrl], UriKind.Absolute, out var url))
                    http.BaseAddress = url;
                http.Timeout = TimeSpan.FromSeconds(3); // RNF: respuesta del panel < 2-3 s
            })
            .AddHttpMessageHandler<ReenviarTokenHandler>();

    Registrar<IUsuariosClient, UsuariosClient>("Servicios:UsuariosUrl");
    Registrar<IMarketplaceClient, MarketplaceClient>("Servicios:MarketplaceUrl");
    Registrar<IResenasClient, ResenasClient>("Servicios:ResenasUrl");
}

// ADMIN Marcos: HU-25 / HU-21 / HU-26 / HU-27 — bitácora, administrador actual y operaciones en servicios dueños.
// Estas operaciones siempre usan los servicios reales (no hay versión de demostración).
builder.Services.AddScoped<IAccionAdministrativaRepository, AccionAdministrativaRepository>();
builder.Services.AddScoped<IUsuarioActual, Admin.Infrastructure.Security.UsuarioActual>();
builder.Services.TryAddTransient<ReenviarTokenHandler>();

void ConfigurarServicio(HttpClient http, string claveUrl)
{
    // URL vacía o inválida: la operación responde "servicio no disponible" (HU-21-04)
    if (Uri.TryCreate(builder.Configuration[claveUrl], UriKind.Absolute, out var url)) http.BaseAddress = url;
    http.Timeout = TimeSpan.FromSeconds(5);
}
builder.Services.AddHttpClient<IUsuariosAdminClient, UsuariosAdminClient>(http => ConfigurarServicio(http, "Servicios:UsuariosUrl"))
    .AddHttpMessageHandler<ReenviarTokenHandler>();
builder.Services.AddHttpClient<IMarketplaceAdminClient, MarketplaceAdminClient>(http => ConfigurarServicio(http, "Servicios:MarketplaceUrl"))
    .AddHttpMessageHandler<ReenviarTokenHandler>();

builder.Services.AddDbContext<AdminDbContext>(options => options.UseNpgsql(connectionString));
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Botón "Authorize" en Swagger para probar con un token de Administrador
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Pega aquí el token JWT (sin la palabra Bearer)"
    });
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});
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

// ADMIN Marcos: aplica las migraciones del schema "admin" al arrancar (igual que Usuarios y Marketplace)
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        scope.ServiceProvider.GetRequiredService<AdminDbContext>().Database.Migrate();
        logger.LogInformation("Migraciones aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al aplicar migraciones de Entity Framework.");
    }
}

app.Run();
