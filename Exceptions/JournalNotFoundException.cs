using System;

namespace journalapi.Services;

public class JournalNotFoundException : Exception
{
    public JournalNotFoundException() { }

    public JournalNotFoundException(string message)
        : base(message) { }

    public JournalNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
