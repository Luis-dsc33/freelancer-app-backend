namespace Resenas.Application.Exceptions;

public sealed class DomainException(string message) : Exception(message);
