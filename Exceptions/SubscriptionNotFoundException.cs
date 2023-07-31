namespace journalapi.Services;

/// <summary>
/// Represents an exception that is thrown when a subscription is not found in the system.
/// </summary>
public class SubscriptionNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the SubscriptionNotFoundException class.
    /// </summary>
    public SubscriptionNotFoundException() { }

    /// <summary>
    /// Initializes a new instance of the SubscriptionNotFoundException class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the reason for the exception.</param>
    public SubscriptionNotFoundException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the SubscriptionNotFoundException class with the specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describes the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public SubscriptionNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
