using System.Text.Json.Serialization;

namespace journalapi.Models;

/// <summary>
/// Represents a subscription relationship between two researchers in the system.
/// </summary>
public class Subscription
{
    /// <summary>
    /// Gets or sets the unique identifier for the subscription.
    /// </summary>
    public Guid SubscriptionId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the researcher who created the subscription.
    /// </summary>
    public Guid ResearcherId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the researcher being followed.
    /// </summary>
    public Guid FollowedResearcherId { get; set; }

    /// <summary>
    /// Gets or sets the researcher who created the subscription. (This property is ignored in API responses.)
    /// </summary>
    [JsonIgnore]
    public virtual Researcher? researcher { get; set; }

    /// <summary>
    /// Gets or sets the researcher being followed. (This property is ignored in API responses.)
    /// </summary>
    [JsonIgnore]
    public virtual Researcher? FollowedResearcher { get; set; }
}
