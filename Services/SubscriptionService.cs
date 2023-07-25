using journalapi.Models;

namespace journalapi.Services;

public class SubscriptionService:ISubscriptionService{
  protected readonly JournalContext context;

  public SubscriptionService(JournalContext contextdb){
    context = contextdb;
  }

  public IEnumerable<Researcher> GetSubscriptors(Guid id){
    var subscriptorsForResearcher = context.Subscriptions.Where<Subscription>(subscription=> subscription.FollowedResearcherId==id);
    var researchers = from researcherSubscripted in subscriptorsForResearcher join researches in context.Researchers on researcherSubscripted.ResearcherId equals researches.ResearcherId select researches; 
    return researchers;
  }

  public IEnumerable<Subscription> GetSubscriptions(Guid id){
    return context.Subscriptions.Where<Subscription>(subscription=> subscription.ResearcherId==id);
    // var researchers = from researcherSubscripted in subscriptionsForResearcher join researches in context.Researchers on researcherSubscripted.FollowedResearcherId equals researches.ResearcherId select researches; 
    // return researchers;
  }

  public IEnumerable<Journal> GetFeed(Guid id){ 
          var result = context.Journals
            .Where(journal => context.Subscriptions.Any(subscription => subscription.ResearcherId == id && subscription.FollowedResearcherId == journal.ResearcherId))
            .OrderBy(journal => journal.PublishedDate)
            .Take(20)
            .ToList();

        return result;
  }

  public async Task<Subscription> Create(Subscription subscription){
    subscription.SubscriptionId = Guid.NewGuid();
    context.Subscriptions.Add(subscription);
    await context.SaveChangesAsync();
    return subscription;
  }

  public async Task Delete(Guid id){
    var currentSubscription = context.Subscriptions.Find(id);
    if(currentSubscription!=null){
      context.Subscriptions.Remove(currentSubscription);
      await context.SaveChangesAsync();
    }
  }



}

public interface ISubscriptionService{
IEnumerable<Researcher> GetSubscriptors(Guid id);
IEnumerable<Subscription> GetSubscriptions(Guid id);
IEnumerable<Journal> GetFeed(Guid id);
Task<Subscription> Create(Subscription subscription);
Task Delete(Guid id);
}