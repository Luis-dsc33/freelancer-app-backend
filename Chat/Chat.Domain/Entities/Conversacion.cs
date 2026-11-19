namespace Chat.Domain.Entities;

public class Conversacion
{
    public Guid Id { get; private set; }
    public Guid ProyectoId { get; private set; }
    public Guid? PostulacionId { get; private set; }
    public Guid? ContratoId { get; private set; }
    public Guid ClienteId { get; private set; }
    public Guid EstudianteId { get; private set; }
    public string? TituloProyecto { get; private set; }
    public string? NombreCliente { get; private set; }
    public DateTime CreadaEn { get; private set; }
    public DateTime UltimaActividadEn { get; private set; }
    public ICollection<Mensaje> Mensajes { get; private set; } = new List<Mensaje>();

    private Conversacion() { }

    public Conversacion(
        Guid proyectoId,
        Guid? postulacionId,
        Guid? contratoId,
        Guid clienteId,
        Guid estudianteId,
        string? tituloProyecto,
        string? nombreCliente = null)
    {
        Id = Guid.NewGuid();
        ProyectoId = proyectoId;
        PostulacionId = postulacionId;
        ContratoId = contratoId;
        ClienteId = clienteId;
        EstudianteId = estudianteId;
        TituloProyecto = tituloProyecto;
        NombreCliente = nombreCliente;
        var ahora = DateTime.UtcNow;
        CreadaEn = ahora;
        UltimaActividadEn = ahora;
    }

    public bool Participa(Guid usuarioId) =>
        usuarioId != Guid.Empty && (ClienteId == usuarioId || EstudianteId == usuarioId);

    public void ActualizarContexto(
        Guid? postulacionId,
        Guid? contratoId,
        string? tituloProyecto,
        string? nombreCliente)
    {
        if (postulacionId.HasValue && !PostulacionId.HasValue)
            PostulacionId = postulacionId;
        if (contratoId.HasValue)
            ContratoId = contratoId;
        if (!string.IsNullOrWhiteSpace(tituloProyecto))
            TituloProyecto = tituloProyecto;
        if (!string.IsNullOrWhiteSpace(nombreCliente))
            NombreCliente = nombreCliente;
    }

    public void RegistrarActividad(DateTime fechaUtc)
    {
        UltimaActividadEn = DateTime.SpecifyKind(fechaUtc, DateTimeKind.Utc);
    }
}
