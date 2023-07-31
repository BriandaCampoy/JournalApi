using System;

namespace journalapi.Services;

public class JournalServiceException : Exception
{
    public JournalServiceException() { }

    public JournalServiceException(string message)
        : base(message) { }

    public JournalServiceException(string message, Exception innerException)
        : base(message, innerException) { }
}
