using System;

namespace journalapi.Services;

/// <summary>
/// Represents an exception that is thrown when a journal is not found in the system.
/// </summary>
public class JournalNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the JournalNotFoundException class.
    /// </summary>
    public JournalNotFoundException() { }

    /// <summary>
    /// Initializes a new instance of the JournalNotFoundException class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the reason for the exception.</param>
    public JournalNotFoundException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the JournalNotFoundException class with the specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describes the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public JournalNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
