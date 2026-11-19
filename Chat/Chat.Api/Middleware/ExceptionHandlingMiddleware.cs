using System.Net;
using System.Text.Json;
using Chat.Application.Exceptions;
using FluentValidation;

namespace Chat.Api.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Error de validación");
            var errors = ex.Errors.Select(error => new
            {
                campo = error.PropertyName,
                mensaje = error.ErrorMessage
            });
            await EscribirJsonAsync(context, HttpStatusCode.BadRequest, new { errores = errors });
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "Regla de dominio rechazada");
            await EscribirJsonAsync(context, HttpStatusCode.BadRequest, new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Regla de negocio rechazada");
            await EscribirJsonAsync(context, HttpStatusCode.BadRequest, new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso denegado o no autorizado");
            var status = context.User?.Identity?.IsAuthenticated == true
                ? HttpStatusCode.Forbidden
                : HttpStatusCode.Unauthorized;
            await EscribirJsonAsync(context, status, new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Recurso no encontrado");
            await EscribirJsonAsync(context, HttpStatusCode.NotFound, new { error = ex.Message });
        }
        catch (ConflictException ex)
        {
            logger.LogWarning(ex, "Conflicto de estado o duplicado");
            await EscribirJsonAsync(context, HttpStatusCode.Conflict, new { error = ex.Message });
        }
        catch (ServicioNoDisponibleException ex)
        {
            logger.LogWarning(ex, "Servicio externo no disponible");
            await EscribirJsonAsync(context, HttpStatusCode.ServiceUnavailable, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error no controlado");
            await EscribirJsonAsync(context, HttpStatusCode.InternalServerError, new
            {
                error = "Ocurrió un error inesperado."
            });
        }
    }

    private static Task EscribirJsonAsync(HttpContext context, HttpStatusCode statusCode, object body)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(JsonSerializer.Serialize(body));
    }
}
