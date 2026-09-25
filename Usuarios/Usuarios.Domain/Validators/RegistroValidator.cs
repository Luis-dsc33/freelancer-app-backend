namespace Usuarios.Domain.Validators;

/// <summary>
/// Validador de lógica pura para el registro de usuarios (HU-01).
/// Valida correo institucional, contraseña segura y nombre.
/// </summary>
public static class RegistroValidator
{
    private static readonly string[] DominiosInstitucionales =
    [
        "@alumnos.udg.mx",
        "@academicos.udg.mx",
        "@udg.mx"
    ];

    /// <summary>
    /// Valida que el correo pertenezca a un dominio institucional permitido.
    /// </summary>
    public static bool EsCorreoInstitucionalValido(string? correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
            return false;

        correo = correo.Trim().ToLowerInvariant();

        // Debe contener exactamente un @
        var partes = correo.Split('@');
        if (partes.Length != 2 || string.IsNullOrWhiteSpace(partes[0]))
            return false;

        return Array.Exists(DominiosInstitucionales,
            dominio => correo.EndsWith(dominio, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Valida que la contraseña cumpla con los requisitos mínimos de seguridad:
    /// - Mínimo 8 caracteres
    /// - Al menos una mayúscula
    /// - Al menos una minúscula
    /// - Al menos un dígito
    /// </summary>
    public static bool EsContrasenaSegura(string? contrasena)
    {
        if (string.IsNullOrWhiteSpace(contrasena))
            return false;

        if (contrasena.Length < 8)
            return false;

        bool tieneMayuscula = false;
        bool tieneMinuscula = false;
        bool tieneDigito = false;

        foreach (var c in contrasena)
        {
            if (char.IsUpper(c)) tieneMayuscula = true;
            else if (char.IsLower(c)) tieneMinuscula = true;
            else if (char.IsDigit(c)) tieneDigito = true;
        }

        return tieneMayuscula && tieneMinuscula && tieneDigito;
    }

    /// <summary>
    /// Valida que el nombre no esté vacío y tenga al menos 2 caracteres.
    /// </summary>
    public static bool EsNombreValido(string? nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return false;

        return nombre.Trim().Length >= 2;
    }

    /// <summary>
    /// Ejecuta todas las validaciones de registro y devuelve el resultado.
    /// </summary>
    public static RegistroValidationResult ValidarRegistro(string? nombre, string? correo, string? contrasena)
    {
        var errores = new List<string>();

        if (!EsNombreValido(nombre))
            errores.Add("El nombre es obligatorio y debe tener al menos 2 caracteres.");

        if (!EsCorreoInstitucionalValido(correo))
            errores.Add("El correo debe pertenecer a un dominio institucional válido (@alumnos.udg.mx, @academicos.udg.mx, @udg.mx).");

        if (!EsContrasenaSegura(contrasena))
            errores.Add("La contraseña debe tener mínimo 8 caracteres, una mayúscula, una minúscula y un dígito.");

        return new RegistroValidationResult
        {
            EsValido = errores.Count == 0,
            Errores = errores
        };
    }
}

public class RegistroValidationResult
{
    public bool EsValido { get; set; }
    public List<string> Errores { get; set; } = [];
}
