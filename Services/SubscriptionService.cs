using journalapi.Models;

namespace journalapi.Services;

/// <summary>
/// Service class for managing subscriptions between researchers.
/// </summary>
public class SubscriptionService : ISubscriptionService
{
    protected readonly JournalContext context;

    public SubscriptionService(JournalContext contextdb)
    {
        context = contextdb;
    }

    /// <summary>
    /// Gets all the researchers that are subscribed to a specific researcher.
    /// </summary>
    /// <param name="id">The researcher ID.</param>
    /// <returns>A list of subscribed researchers.</returns>
    public IEnumerable<Researcher> GetSubscriptors(Guid id)
    {
        try
        {
            if (context.Journals.Find(id) == null)
            {
                throw new SubscriptionNotFoundException("Researcher not found");
            }
            var subscriptorsForResearcher = context.Subscriptions.Where<Subscription>(
                subscription => subscription.FollowedResearcherId == id
            );
            var researchers =
                from researcherSubscripted in subscriptorsForResearcher
                join researches in context.Researchers
                    on researcherSubscripted.ResearcherId equals researches.ResearcherId
                select researches;
            return researchers;
        }
        catch (SubscriptionNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new SubscriptionServiceException("Error while retrieving subscriptors", ex);
        }
    }

    /// <summary>
    /// Gets all the subscriptions of a specific researcher.
    /// </summary>
    /// <param name="id">The researcher ID.</param>
    /// <returns>A list of subscriptions.</returns>
    public IEnumerable<Subscription> GetSubscriptions(Guid id)
    {
        try
        {
            if (context.Journals.Find(id) == null)
            {
                throw new SubscriptionNotFoundException("Researcher not found");
            }
            return context.Subscriptions.Where<Subscription>(
                subscription => subscription.ResearcherId == id
            );
        }
        catch (SubscriptionNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new SubscriptionServiceException("Error while retrieving subscriptions", ex);
        }
    }

    /// <summary>
    /// Gets the feed of journals from researchers the specified researcher is subscribed to.
    /// </summary>
    /// <param name="id">The researcher ID.</param>
    /// <returns>The feed of journals.</returns>
    public IEnumerable<Journal> GetFeed(Guid id)
    {
        try
        {
            if (context.Journals.Find(id) == null)
            {
                throw new SubscriptionNotFoundException("Researcher not found");
            }
            var result = context.Journals
                .Where(
                    journal =>
                        context.Subscriptions.Any(
                            subscription =>
                                subscription.ResearcherId == id
                                && subscription.FollowedResearcherId == journal.ResearcherId
                        )
                )
                .OrderBy(journal => journal.PublishedDate)
                .Take(20)
                .ToList();

            return result;
        }
        catch (SubscriptionNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new SubscriptionServiceException("Error while retrieving feed", ex);
        }
    }

    /// <summary>
    /// Creates a new subscription between researchers.
    /// </summary>
    /// <param name="subscription">The subscription object to create.</param>
    /// <returns>The newly created subscription.</returns>
    public async Task<Subscription> Create(Subscription subscription)
    {
        try
        {
            subscription.SubscriptionId = Guid.NewGuid();
            context.Subscriptions.Add(subscription);
            await context.SaveChangesAsync();
            return subscription;
        }
        catch (Exception ex)
        {
            throw new SubscriptionServiceException("Error while creating subscription", ex);
        }
    }

    /// <summary>
    /// Deletes a specific subscription.
    /// </summary>
    /// <param name="id">The subscription ID to delete.</param>
    public async Task Delete(Guid id)
    {
        try
        {
            var currentSubscription = context.Subscriptions.Find(id);

            if (currentSubscription == null)
            {
                throw new SubscriptionNotFoundException("Researcher not found");
            }
            else
            {
                context.Subscriptions.Remove(currentSubscription);
                await context.SaveChangesAsync();
            }
        }
        catch (SubscriptionNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new SubscriptionServiceException("Error while deleting subscription", ex);
        }
    }
}

public interface ISubscriptionService
{
    IEnumerable<Researcher> GetSubscriptors(Guid id);
    IEnumerable<Subscription> GetSubscriptions(Guid id);
    IEnumerable<Journal> GetFeed(Guid id);
    Task<Subscription> Create(Subscription subscription);
    Task Delete(Guid id);
}
