using System;

namespace journalapi.Services;

/// <summary>
/// Represents an exception that is thrown when an error occurs in the ResearcherService.
/// </summary>
public class ResearcherServiceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ResearcherServiceException class.
    /// </summary>
    public ResearcherServiceException() { }

    /// <summary>
    /// Initializes a new instance of the ResearcherServiceException class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the reason for the exception.</param>
    public ResearcherServiceException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the ResearcherServiceException class with the specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describes the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ResearcherServiceException(string message, Exception innerException)
        : base(message, innerException) { }
}
