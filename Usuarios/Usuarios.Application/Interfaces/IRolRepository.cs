using Usuarios.Domain.Entities;

namespace Usuarios.Application.Interfaces;

public interface IRolRepository
{
    Task<Rol?> ObtenerPorNombreAsync(string nombre);
}