using System.Text.Json.Serialization;
namespace journalapi.Models;

public class Researcher{

  public Guid ResearcherId { get; set; }

  public string Name { get; set; } = string.Empty;

  public string Email { get; set; } = string.Empty;

  public string Password { get; set; }= string.Empty;

[JsonIgnore]
  public virtual ICollection<Journal>? Journals { get; set; }

[JsonIgnore]
  public virtual ICollection<Subscription>?Subscriptions{get; set; }

[JsonIgnore]
  public virtual ICollection<Subscription>?Subscriptors{get; set; }

}