using Usuarios.Domain.Validators;
using Xunit;

namespace Usuarios.Tests;

/// <summary>
/// Pruebas unitarias para RegistroValidator (HU-01).
/// Valida correo institucional, contraseña segura y nombre.
/// </summary>
public class RegistroValidatorTests
{
    // ─────────────────────────────────────────────
    //  Correo institucional
    // ─────────────────────────────────────────────

    [Theory]
    [InlineData("luis.dominguez@alumnos.udg.mx")]
    [InlineData("profesor@academicos.udg.mx")]
    [InlineData("admin@udg.mx")]
    public void EsCorreoInstitucionalValido_CorreoValido_RetornaTrue(string correo)
    {
        Assert.True(RegistroValidator.EsCorreoInstitucionalValido(correo));
    }

    [Theory]
    [InlineData("usuario@gmail.com")]
    [InlineData("usuario@hotmail.com")]
    [InlineData("usuario@outlook.com")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("correo-sin-arroba")]
    [InlineData("@alumnos.udg.mx")]
    public void EsCorreoInstitucionalValido_CorreoInvalido_RetornaFalse(string? correo)
    {
        Assert.False(RegistroValidator.EsCorreoInstitucionalValido(correo));
    }

    [Fact]
    public void EsCorreoInstitucionalValido_CorreoConMayusculas_RetornaTrue()
    {
        Assert.True(RegistroValidator.EsCorreoInstitucionalValido("LUIS@ALUMNOS.UDG.MX"));
    }

    // ─────────────────────────────────────────────
    //  Contraseña segura
    // ─────────────────────────────────────────────

    [Theory]
    [InlineData("Segura123")]
    [InlineData("MiPassw0rd")]
    [InlineData("Abcdefg1")]
    public void EsContrasenaSegura_ContrasenaValida_RetornaTrue(string contrasena)
    {
        Assert.True(RegistroValidator.EsContrasenaSegura(contrasena));
    }

    [Theory]
    [InlineData("corta1A")]         // menos de 8 caracteres
    [InlineData("sinmayusculas1")]  // sin mayúsculas
    [InlineData("SINMINUSCULAS1")]  // sin minúsculas
    [InlineData("SinDigitos")]      // sin dígitos
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void EsContrasenaSegura_ContrasenaInvalida_RetornaFalse(string? contrasena)
    {
        Assert.False(RegistroValidator.EsContrasenaSegura(contrasena));
    }

    // ─────────────────────────────────────────────
    //  Nombre válido
    // ─────────────────────────────────────────────

    [Theory]
    [InlineData("Luis")]
    [InlineData("Ma")]
    [InlineData("Ana García")]
    public void EsNombreValido_NombreValido_RetornaTrue(string nombre)
    {
        Assert.True(RegistroValidator.EsNombreValido(nombre));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("A")]
    public void EsNombreValido_NombreInvalido_RetornaFalse(string? nombre)
    {
        Assert.False(RegistroValidator.EsNombreValido(nombre));
    }

    // ─────────────────────────────────────────────
    //  Validación completa de registro
    // ─────────────────────────────────────────────

    [Fact]
    public void ValidarRegistro_DatosValidos_EsValidoTrue()
    {
        var resultado = RegistroValidator.ValidarRegistro(
            "Luis Domínguez",
            "luis@alumnos.udg.mx",
            "Segura123"
        );

        Assert.True(resultado.EsValido);
        Assert.Empty(resultado.Errores);
    }

    [Fact]
    public void ValidarRegistro_TodoInvalido_RetornaTresErrores()
    {
        var resultado = RegistroValidator.ValidarRegistro(
            "",
            "correo@gmail.com",
            "123"
        );

        Assert.False(resultado.EsValido);
        Assert.Equal(3, resultado.Errores.Count);
    }

    [Fact]
    public void ValidarRegistro_SoloCorreoInvalido_RetornaUnError()
    {
        var resultado = RegistroValidator.ValidarRegistro(
            "Luis",
            "luis@gmail.com",
            "Segura123"
        );

        Assert.False(resultado.EsValido);
        Assert.Single(resultado.Errores);
        Assert.Contains("dominio institucional", resultado.Errores[0]);
    }
}
