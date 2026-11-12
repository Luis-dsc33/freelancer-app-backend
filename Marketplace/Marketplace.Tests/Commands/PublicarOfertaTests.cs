using FluentValidation.TestHelper;
using Marketplace.Application.Commands.PublicarOferta;
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Moq;

namespace Marketplace.Tests.Commands;

public class PublicarOfertaTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validator_PresupuestoNoPositivo_DevuelveError(decimal presupuesto)
    {
        var result = new PublicarOfertaCommandValidator()
            .TestValidate(ValidCommand(presupuesto: presupuesto));

        result.ShouldHaveValidationErrorFor(command => command.Presupuesto);
    }

    [Fact]
    public void Validator_FechaAnteriorAHoy_DevuelveError()
    {
        var result = new PublicarOfertaCommandValidator()
            .TestValidate(ValidCommand(fechaLimite: DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1)));

        result.ShouldHaveValidationErrorFor(command => command.FechaLimite);
    }

    [Fact]
    public void Validator_PresupuestoDeUnCentavo_EsValido()
    {
        var result = new PublicarOfertaCommandValidator()
            .TestValidate(ValidCommand(presupuesto: 0.01m));

        result.ShouldNotHaveValidationErrorFor(command => command.Presupuesto);
    }

    [Fact]
    public void Validator_TituloDescripcionYDisciplinaVacios_DevuelveErrores()
    {
        var command = ValidCommand() with { Titulo = "", Descripcion = "", Disciplina = "" };
        var result = new PublicarOfertaCommandValidator().TestValidate(command);

        result.ShouldHaveValidationErrorFor(item => item.Titulo);
        result.ShouldHaveValidationErrorFor(item => item.Descripcion);
        result.ShouldHaveValidationErrorFor(item => item.Disciplina);
    }

    [Fact]
    public async Task Handler_OfertaValida_GuardaYDevuelveLaOfertaAbierta()
    {
        var repository = new Mock<IProyectoRepository>();
        Proyecto? guardado = null;
        repository
            .Setup(item => item.AgregarAsync(It.IsAny<Proyecto>(), It.IsAny<CancellationToken>()))
            .Callback<Proyecto, CancellationToken>((proyecto, _) => guardado = proyecto)
            .Returns(Task.CompletedTask);
        var handler = new PublicarOfertaHandler(repository.Object);
        var command = ValidCommand();

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(guardado);
        Assert.Equal(result.Id, guardado!.Id);
        Assert.Equal(command.ClienteId, guardado.ClienteId);
        Assert.Equal(command.Disciplina, result.Disciplina);
        Assert.Equal(command.FechaLimite, result.FechaLimite);
        Assert.Equal(Proyecto.EstadoAbierto, result.Estado);
        repository.Verify(
            item => item.AgregarAsync(It.IsAny<Proyecto>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static PublicarOfertaCommand ValidCommand(
        decimal presupuesto = 500m,
        DateOnly? fechaLimite = null) =>
        new(
            "Sitio web",
            "Crear un sitio web informativo",
            "Desarrollo de software",
            presupuesto,
            fechaLimite ?? DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1),
            Guid.NewGuid(),
            "Cliente de prueba");
}
