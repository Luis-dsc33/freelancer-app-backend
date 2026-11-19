using FluentValidation;

namespace Chat.Application.Commands.EnviarMensajeChat;

public sealed class EnviarMensajeChatCommandValidator : AbstractValidator<EnviarMensajeChatCommand>
{
    public EnviarMensajeChatCommandValidator()
    {
        RuleFor(x => x.ConversacionId)
            .NotEmpty().WithMessage("El identificador de la conversación es obligatorio.");

        RuleFor(x => x.Contenido)
            .NotEmpty().WithMessage("El mensaje no puede estar vacío.")
            .Must(c => !string.IsNullOrWhiteSpace(c) && c.Trim().Length <= 2000)
            .WithMessage("El mensaje no puede exceder los 2000 caracteres.");
    }
}
