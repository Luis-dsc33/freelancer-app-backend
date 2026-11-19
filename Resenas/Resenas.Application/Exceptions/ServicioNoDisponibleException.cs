namespace Resenas.Application.Exceptions;

public sealed class ServicioNoDisponibleException(string servicio, Exception? inner = null)
    : Exception($"El servicio '{servicio}' no está disponible en este momento.", inner);
