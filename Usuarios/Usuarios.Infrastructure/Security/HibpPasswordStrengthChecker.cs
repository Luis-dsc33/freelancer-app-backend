using System.Security.Cryptography;
using System.Text;
using Usuarios.Application.Interfaces;

namespace Usuarios.Infrastructure.Security;

public class HibpPasswordStrengthChecker : IPasswordStrengthChecker
{
    private readonly HttpClient _httpClient;

    public HibpPasswordStrengthChecker(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool EsDebil, string Razon)> EsDebilAsync(string password, CancellationToken ct = default)
    {
        //OBTIENE EL HASH COMPLETO DE LA CONTRASEÑA EN MAYUSCULA 
        var sha1Bytes = SHA1.HashData(Encoding.UTF8.GetBytes(password));
        var sha1Hash = Convert.ToHexString(sha1Bytes).ToUpperInvariant();

        //EL MODELO k-Anonymity DE HIBP (SE ENCARGA DE REALIZAR AGRUPACIONES DE HASHES): CON ESTE SE SEPARA LOS PRIMEROS CINCO CARACTERES
        var prefijo = sha1Hash[..5];
        var sufijoEsperado = sha1Hash[5..];

        try{
            // SE CONSULTA LA API DE HIBP ENVIANDO ESOS 5 CARACTERES QUE SE RECUPERARON ANTES 
            var respuesta = await _httpClient.GetAsync($"https://api.pwnedpasswords.com/range/{prefijo}", ct);

            if (!respuesta.IsSuccessStatusCode){
                // SI LA API EXTERNA LLEGA A FALLAR, SE PERMITE CONTINUAR
                return (false, string.Empty);
            }
            var contenido = await respuesta.Content.ReadAsStringAsync(ct);

            // SE VERIFICA QUE EL SUFIJO COINCIDA CON ALGUNAS DE LAS RESPUESTAS DEVUELTAS 
            using var reader = new StringReader(contenido);
            string? linea;

            while ((linea = await reader.ReadLineAsync(ct)) is not null){
                var partes = linea.Split(':');
                if (partes.Length == 2 && partes[0].Equals(sufijoEsperado, StringComparison.OrdinalIgnoreCase)){
                    var repeticiones = partes[1].Trim();
                    return (true, $"Esta contraseña ha sido expuesta en filtraciones de datos públicas. Por tu seguridad, elige una contraseña diferente.");
                }
            }
        }
        catch{
            return (false, string.Empty);
        }
        return (false, string.Empty);
    }
}