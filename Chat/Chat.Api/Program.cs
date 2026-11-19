using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using MediatR;
using Chat.Application.Behaviors;
using Chat.Application.Interfaces;
using Chat.Api.Hubs;
using Chat.Api.Middleware;
using Chat.Infrastructure.Clients;
using Chat.Infrastructure.Persistence;
using Chat.Infrastructure.Security;
using Microsoft.AspNetCore.SignalR;
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

builder.Services.AddDbContext<ChatDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IConversacionRepository, ConversacionRepository>();
builder.Services.AddScoped<IMensajeRepository, MensajeRepository>();
builder.Services.AddScoped<IChatNotifier, SignalRChatNotifier>();
builder.Services.AddSingleton<IUserIdProvider, JwtUserIdProvider>();
builder.Services.AddTransient<JwtForwardingHandler>();

builder.Services.AddHttpClient<IMarketplaceChatClient, MarketplaceChatClient>(http =>
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

builder.Services.AddSignalR();

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
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/chat"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
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
              .AllowAnyMethod()
              .AllowCredentials();
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
app.MapHub<ChatHub>("/hubs/chat");
app.MapGet("/health", () => Results.Ok(new { status = "healthy" })).AllowAnonymous().ExcludeFromDescription();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
        if (db.Database.IsRelational())
        {
            db.Database.Migrate();
            logger.LogInformation("Migraciones de Chat aplicadas correctamente.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al aplicar migraciones de Entity Framework en Chat.");
    }
}

app.Run();
