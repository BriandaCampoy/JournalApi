using System.Text.Json.Serialization;

namespace journalapi.Models;


public class Journal{
  public Guid JournalId { get; set; }
  public Guid ResearcherId {get; set; }
  public string? Url { get; set; }
  public string Title { get; set; }
  public DateTime? PublishedDate { get; set; }
  public IFormFile? journalFile { get; set; }
[JsonIgnore]
  public string? InternalUrl { get; set; }
[JsonIgnore]
  public virtual Researcher? Researcher{get;set;}
}