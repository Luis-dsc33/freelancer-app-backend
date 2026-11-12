using System.Net;
using System.Text.Json;
using FluentValidation;

namespace Admin.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validación");
            var errores = ex.Errors.Select(e => new { campo = e.PropertyName, mensaje = e.ErrorMessage });
            await EscribirAsync(context, HttpStatusCode.BadRequest, new { errores });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Regla de negocio rechazada");
            await EscribirAsync(context, HttpStatusCode.BadRequest, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado");
            // Nunca ex.Message aquí: evita filtrar detalles internos
            await EscribirAsync(context, HttpStatusCode.InternalServerError, new { error = "Ocurrió un error inesperado." });
        }
    }

    private static Task EscribirAsync(HttpContext context, HttpStatusCode codigo, object cuerpo)
    {
        context.Response.StatusCode = (int)codigo;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(JsonSerializer.Serialize(cuerpo));
    }
}
