using System.Net;
using System.Text.Json;
using FluentValidation;
using Marketplace.Application.Exceptions;

namespace Marketplace.Api.Middleware;

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
            await EscribirJsonAsync(context, HttpStatusCode.BadRequest, new { errores });
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Regla de dominio rechazada");
            await EscribirJsonAsync(context, HttpStatusCode.BadRequest, new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Regla de negocio rechazada");
            await EscribirJsonAsync(context, HttpStatusCode.BadRequest, new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Acceso denegado o no autorizado");
            var status = context.User?.Identity?.IsAuthenticated == true
                ? HttpStatusCode.Forbidden
                : HttpStatusCode.Unauthorized;
            await EscribirJsonAsync(context, status, new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso no encontrado");
            await EscribirJsonAsync(context, HttpStatusCode.NotFound, new { error = ex.Message });
        }
        catch (ConflictException ex)
        {
            _logger.LogWarning(ex, "Conflicto con el estado actual");
            await EscribirJsonAsync(context, HttpStatusCode.Conflict, new { error = ex.Message });
        }
        catch (ServicioNoDisponibleException ex)
        {
            _logger.LogWarning(ex, "Servicio dependiente no disponible");
            await EscribirJsonAsync(context, HttpStatusCode.ServiceUnavailable, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado");
            // Nunca ex.Message aquí — evita filtrar detalles internos (rutas, stack, nombres de tablas)
            await EscribirJsonAsync(context, HttpStatusCode.InternalServerError, new { error = "Ocurrió un error inesperado." });
        }
    }

    private static Task EscribirJsonAsync(HttpContext context, HttpStatusCode statusCode, object body)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(JsonSerializer.Serialize(body));
    }
}
