
namespace journalapi.Services;
public class ResearcherNotFoundException : Exception
{
    public ResearcherNotFoundException() { }

    public ResearcherNotFoundException(string message)
        : base(message) { }

    public ResearcherNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
