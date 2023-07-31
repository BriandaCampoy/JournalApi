using System.Text.Json.Serialization;

namespace journalapi.Models;

/// <summary>
/// Represents a journal entry in the system.
/// </summary>
public class Journal
{
    /// <summary>
    /// Gets or sets the unique identifier for the journal.
    /// </summary>
    public Guid JournalId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the researcher associated with the journal.
    /// </summary>
    public Guid ResearcherId { get; set; }

    /// <summary>
    /// Gets or sets the URL of the journal.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets the title of the journal.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the published date of the journal.
    /// </summary>
    public DateTime? PublishedDate { get; set; }

    /// <summary>
    /// Gets or sets the journal file uploaded by the researcher. (Note: IFormFile is part of ASP.NET Core.)
    /// </summary>
    public IFormFile? journalFile { get; set; }

    /// <summary>
    /// Gets or sets the internal URL of the journal. (This property is ignored in API responses.)
    /// </summary>
    [JsonIgnore]
    public string? InternalUrl { get; set; }

    /// <summary>
    /// Gets or sets the virtual researcher associated with the journal. (This property is ignored in API responses.)
    /// </summary>
    [JsonIgnore]
    public virtual Researcher? Researcher { get; set; }
}
