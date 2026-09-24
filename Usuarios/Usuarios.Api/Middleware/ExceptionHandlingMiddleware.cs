using System.Net;
using System.Text.Json;
using FluentValidation;
using Usuarios.Application.Exceptions;

namespace Usuarios.Api.Middleware;

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
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var errores = ex.Errors.Select(e => new { campo = e.PropertyName, mensaje = e.ErrorMessage });
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { errores }));
        }
        catch (EmailDeliveryException ex)
        {
            _logger.LogError(ex, "No se pudo completar el envío del correo de recuperación");
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = "No se pudo procesar la solicitud en este momento. Inténtalo más tarde."
            }));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Regla de negocio rechazada");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            // Nunca ex.Message aquí — evita filtrar detalles internos (rutas, stack, nombres de tablas)
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Ocurrió un error inesperado." }));
        }
    }
}
