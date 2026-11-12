using Moq;
using System.Timers;
using Usuarios.Application.Commands.RegistrarUsuario;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;
using Xunit;

namespace Usuarios.Tests.Commands;

public class RegistrarUsuarioHandlerDuplicateTests
{
    // xUnit crea una instancia nueva de esta clase por cada prueba,
    // así que estos mocks siempre arrancan limpios.
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly Mock<IRolRepository> _rolRepo = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly RegistrarUsuarioHandler _handler;

    public RegistrarUsuarioHandlerDuplicateTests()
    {
        _handler = new RegistrarUsuarioHandler(_usuarioRepo.Object, _rolRepo.Object, _hasher.Object);
    }

    // PRUEBA 1 — HU-01, Escenario 01-01: registro válido
    [Fact]
    public async Task Handle_DatosValidos_DevuelveIdYGuardaUnUsuario()
    {
        // ARRANGE: el correo no existe, el rol sí existe y el hasher devuelve un hash simulado
        var rol = new Rol { Id = Guid.NewGuid(), Nombre = "Estudiante" };
        var command = new RegistrarUsuarioCommand("Ana López", "ana@utsjr.edu.mx", "Segura123!", "Estudiante");

        _usuarioRepo.Setup(r => r.ExisteEmailAsync("ana@utsjr.edu.mx")).ReturnsAsync(false);
        _rolRepo.Setup(r => r.ObtenerPorNombreAsync("Estudiante")).ReturnsAsync(rol);
        _hasher.Setup(h => h.Hash("Segura123!")).Returns("HASH_SIMULADO");

        // ACT: ejecutamos el Handler
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // ASSERT: devuelve un Id válido y se guardó exactamente un usuario con ese Id
        Assert.NotEqual(Guid.Empty, resultado);
        _usuarioRepo.Verify(r => r.AgregarAsync(It.Is<Usuario>(u => u.Id == resultado)), Times.Once);
    }

    // PRUEBA 2 — HU-01, Escenario 01-02: correo duplicado
    [Fact]
    public async Task Handle_CorreoYaRegistrado_LanzaExcepcionYNoGuarda()
    {
        // ARRANGE: el repositorio indica que el correo ya existe
        var command = new RegistrarUsuarioCommand("Ana López", "ana@utsjr.edu.mx", "Segura123!", "Estudiante");
        _usuarioRepo.Setup(r => r.ExisteEmailAsync("ana@utsjr.edu.mx")).ReturnsAsync(true);

        // ACT: ejecutamos esperando que lance la excepción
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        // ASSERT: mensaje correcto y nunca se llamó a guardar
        Assert.Equal("El correo ya está registrado.", ex.Message);
        _usuarioRepo.Verify(r => r.AgregarAsync(It.IsAny<Usuario>()), Times.Never);
    }

    // PRUEBA 3 — HU-22: el rol se valida en el servidor
    [Fact]
    public async Task Handle_RolInexistente_LanzaExcepcionYNoGuarda()
    {
        // ARRANGE: el correo está libre, pero el rol no existe en la base de datos
        var command = new RegistrarUsuarioCommand("Ana López", "ana@utsjr.edu.mx", "Segura123!", "RolFalso");
        _usuarioRepo.Setup(r => r.ExisteEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
        _rolRepo.Setup(r => r.ObtenerPorNombreAsync("RolFalso")).ReturnsAsync((Rol?)null);

        // ACT
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        // ASSERT: mensaje correcto y no se guardó nada
        Assert.Equal("El rol indicado no es válido.", ex.Message);
        _usuarioRepo.Verify(r => r.AgregarAsync(It.IsAny<Usuario>()), Times.Never);
    }

    // PRUEBA 4 — HU-22: la contraseña nunca se guarda en texto plano
    [Fact]
    public async Task Handle_DatosValidos_GuardaLaContrasenaHasheadaNuncaEnTextoPlano()
    {
        // ARRANGE: capturamos el usuario que el Handler entrega al repositorio
        const string passwordPlano = "Segura123!";
        var rol = new Rol { Id = Guid.NewGuid(), Nombre = "Estudiante" };
        var command = new RegistrarUsuarioCommand("Ana López", "ana@utsjr.edu.mx", passwordPlano, "Estudiante");
        Usuario? usuarioGuardado = null;

        _usuarioRepo.Setup(r => r.ExisteEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
        _rolRepo.Setup(r => r.ObtenerPorNombreAsync("Estudiante")).ReturnsAsync(rol);
        _hasher.Setup(h => h.Hash(passwordPlano)).Returns("HASH_SIMULADO");
        _usuarioRepo.Setup(r => r.AgregarAsync(It.IsAny<Usuario>()))
            .Callback<Usuario>(u => usuarioGuardado = u)
            .Returns(Task.CompletedTask);

        // ACT
        await _handler.Handle(command, CancellationToken.None);

        // ASSERT: se guardó el hash que devolvió el hasher, no la contraseña original
        Assert.NotNull(usuarioGuardado);
        Assert.Equal("HASH_SIMULADO", usuarioGuardado!.PasswordHash);
        Assert.NotEqual(passwordPlano, usuarioGuardado.PasswordHash);
    }

    // PRUEBA 5 — HU-02 y HU-22: estado y rol los define el servidor
    [Fact]
    public async Task Handle_DatosValidos_CreaElUsuarioActivoConElRolEncontrado()
    {
        // ARRANGE
        var rol = new Rol { Id = Guid.NewGuid(), Nombre = "Cliente" };
        var command = new RegistrarUsuarioCommand("Luis Pérez", "luis@utsjr.edu.mx", "Segura123!", "Cliente");
        Usuario? usuarioGuardado = null;

        _usuarioRepo.Setup(r => r.ExisteEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
        _rolRepo.Setup(r => r.ObtenerPorNombreAsync("Cliente")).ReturnsAsync(rol);
        _hasher.Setup(h => h.Hash(It.IsAny<string>())).Returns("HASH_SIMULADO");
        _usuarioRepo.Setup(r => r.AgregarAsync(It.IsAny<Usuario>()))
            .Callback<Usuario>(u => usuarioGuardado = u)
            .Returns(Task.CompletedTask);

        // ACT
        await _handler.Handle(command, CancellationToken.None);

        // ASSERT: arranca como "Activo" (si no, no podría iniciar sesión) y con el RolId del rol buscado
        Assert.NotNull(usuarioGuardado);
        Assert.Equal("Activo", usuarioGuardado!.Estado);
        Assert.Equal(rol.Id, usuarioGuardado.RolId);
        Assert.Equal("Luis Pérez", usuarioGuardado.Nombre);
        Assert.Equal("luis@utsjr.edu.mx", usuarioGuardado.Email);
    }
}