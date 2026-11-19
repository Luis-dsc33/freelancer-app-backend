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
        // ADMIN Marcos: HU-21-04 el servicio dueño no respondió → 503, la medida no se aplicó
        catch (Admin.Application.Exceptions.ServicioNoDisponibleException ex)
        {
            _logger.LogWarning(ex, "Servicio externo no disponible");
            await EscribirAsync(context, HttpStatusCode.ServiceUnavailable, new { error = ex.Message });
        }
        // ADMIN Marcos: token sin identificador de administrador → 401
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Administrador no identificado");
            await EscribirAsync(context, HttpStatusCode.Unauthorized, new { error = "No se pudo identificar al administrador." });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Regla de negocio rechazada");
            await EscribirAsync(context, HttpStatusCode.BadRequest, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado");
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
