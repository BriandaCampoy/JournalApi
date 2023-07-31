using System.Text.Json.Serialization;

namespace journalapi.Models;

/// <summary>
/// Represents a researcher in the system.
/// </summary>
public class Researcher
{
    /// <summary>
    /// Gets or sets the unique identifier for the researcher.
    /// </summary>
    public Guid ResearcherId { get; set; }

    /// <summary>
    /// Gets or sets the name of the researcher.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the researcher.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password of the researcher.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of journals associated with the researcher. (This property is ignored in API responses.)
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Journal>? Journals { get; set; }

    /// <summary>
    /// Gets or sets the collection of subscriptions associated with the researcher. (This property is ignored in API responses.)
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Subscription>? Subscriptions { get; set; }

    /// <summary>
    /// Gets or sets the collection of researchers who have subscribed to this researcher. (This property is ignored in API responses.)
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Subscription>? Subscriptors { get; set; }
}
