using Usuarios.Domain.Entities;

namespace Usuarios.Application.Interfaces;

public interface IJwtService
{
    (string Token, DateTime ExpiraEn) GenerarToken(Usuario usuario);
}