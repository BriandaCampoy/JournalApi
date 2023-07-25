using System.Text.Json.Serialization;

namespace journalapi.Models;

public class Subscription
{
    public Guid SubscriptionId { get; set; }
    public Guid ResearcherId { get; set; }
    public Guid FollowedResearcherId { get; set; }

    [JsonIgnore]
    public virtual Researcher? researcher { get; set; }
    [JsonIgnore]
    public virtual Researcher? FollowedResearcher { get; set; }
}
