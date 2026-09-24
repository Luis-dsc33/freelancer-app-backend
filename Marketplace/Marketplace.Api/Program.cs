var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// -- EMMA 23/09/2026: SE AGREGO LO DE CORS PARA QUE EL FRONT PUEDA CONSUMIR EL API
// CONFIG DE CORS

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("FrontendPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
