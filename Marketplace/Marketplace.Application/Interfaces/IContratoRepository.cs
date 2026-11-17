using Marketplace.Domain.Entities;

namespace Marketplace.Application.Interfaces;

public interface IContratoRepository
{
    Task<bool> ExisteParaPostulacionAsync(Guid postulacionId, CancellationToken cancellationToken);
    Task AgregarAsync(Contrato contrato, CancellationToken cancellationToken);
    Task<List<Contrato>> ObtenerPorClienteAsync(Guid clienteId, CancellationToken cancellationToken);
    Task<Contrato?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task ActualizarAsync(Contrato contrato, CancellationToken cancellationToken);
}
