using System;
namespace journalapi.Services;
public class ResearcherServiceException : Exception
{
    public ResearcherServiceException() { }

    public ResearcherServiceException(string message)
        : base(message) { }

    public ResearcherServiceException(string message, Exception innerException)
        : base(message, innerException) { }
}


