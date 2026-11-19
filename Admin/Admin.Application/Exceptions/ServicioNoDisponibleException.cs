// ADMIN Marcos: HU-21-04 el servicio dueño del dato no respondió; la medida no se aplica ni se registra.
namespace Admin.Application.Exceptions;

public class ServicioNoDisponibleException(string servicio, Exception? interna = null)
    : Exception($"El servicio de {servicio} no está disponible en este momento. La medida no se aplicó.", interna);
