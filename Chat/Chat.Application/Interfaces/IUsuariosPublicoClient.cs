namespace Chat.Application.Interfaces;

public interface IUsuariosPublicoClient
{
    Task<IReadOnlyDictionary<Guid, string>> ObtenerNombresPorIdsAsync(
        IEnumerable<Guid> usuarioIds,
        CancellationToken cancellationToken);
}
