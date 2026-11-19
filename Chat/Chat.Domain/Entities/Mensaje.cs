namespace Chat.Domain.Entities;

public class Mensaje
{
    public Guid Id { get; private set; }
    public Guid ConversacionId { get; private set; }
    public Guid RemitenteId { get; private set; }
    public string Contenido { get; private set; } = string.Empty;
    public DateTime EnviadoEn { get; private set; }
    public Conversacion Conversacion { get; private set; } = default!;

    private Mensaje() { }

    public Mensaje(Guid conversacionId, Guid remitenteId, string contenido)
    {
        Id = Guid.NewGuid();
        ConversacionId = conversacionId;
        RemitenteId = remitenteId;
        Contenido = contenido;
        EnviadoEn = DateTime.UtcNow;
    }
}
