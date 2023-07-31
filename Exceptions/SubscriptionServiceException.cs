namespace journalapi.Services;

public class SubscriptionServiceException : Exception
{
    public SubscriptionServiceException() { }

    public SubscriptionServiceException(string message)
        : base(message) { }

    public SubscriptionServiceException(string message, Exception innerException)
        : base(message, innerException) { }
}
