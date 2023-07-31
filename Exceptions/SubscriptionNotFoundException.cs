namespace journalapi.Services;

public class SubscriptionNotFoundException : Exception
{
    public SubscriptionNotFoundException() { }

    public SubscriptionNotFoundException(string message)
        : base(message) { }

    public SubscriptionNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
