namespace Fintrak.CustomerPortal.Application.Common.Exceptions;

public class EntityDuplicatedException : Exception
{
    public EntityDuplicatedException()
        : base()
    {
    }

    public EntityDuplicatedException(string message)
        : base(message)
    {
    }

    public EntityDuplicatedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public EntityDuplicatedException(string name, object key)
        : base($"Entity \"{name}\" ({key}) already exist.")
    {
    }
}
