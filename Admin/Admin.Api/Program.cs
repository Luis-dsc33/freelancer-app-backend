using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    builder.Services.AddScoped<ICasosClient, CasosFakeClient>();
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
    Registrar<ICasosClient, CasosClient>("Servicios:CasosUrl");
}

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
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

// -- EMMA 23/09/2026: SE AGREGO LO DE CORS PARA QUE EL FRONT PUEDA CONSUMIR EL API
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:8100",             // ESTE ES DESARROLLO LOCAL
                "https://falta-dominio-frontend.com"    // AJUSTAMOS ESTE PA CUANDO YA ESTE ARRIBA
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

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

app.Run();
