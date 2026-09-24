using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;   // JwtSecurityToken, JwtSecurityTokenHandler, JwtRegisteredClaimNames
using Microsoft.IdentityModel.Tokens;    // SymmetricSecurityKey, SigningCredentials, SecurityAlgorithms
using System.Security.Claims;
using System.Text;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Security;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    public JwtService(IConfiguration config) => _config = config;

    public (string Token, DateTime ExpiraEn) GenerarToken(Usuario usuario)
    {
        var minutos = int.Parse(_config["Jwt:ExpiraMinutos"]!);
        var expiraEn = DateTime.UtcNow.AddMinutes(minutos);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Role, usuario.Rol.Nombre)   // "Estudiante" / "Cliente" / "Administrador"
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiraEn,
            signingCredentials: credenciales
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiraEn);
    }
}