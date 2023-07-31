using Microsoft.AspNetCore.Mvc;
using journalapi.Models;
using journalapi.Services;

namespace journalapi.Controllers;

/// <summary>
/// Controller for managing subscriptions between researchers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ILogger<SubscriptionController> _logger;

    protected readonly ISubscriptionService subscriptionService;

    public SubscriptionController(
        ISubscriptionService service,
        ILogger<SubscriptionController> logger
    )
    {
        subscriptionService = service;
        _logger = logger;
    }

    /// <summary>
    /// Gets all the researchers that are subscribed to a specific researcher.
    /// </summary>
    /// <param name="id">The researcher ID.</param>
    /// <returns>A list of subscribed researchers.</returns>
    [HttpGet("subscriptors/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Researcher>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetSubscriptors(Guid id)
    {
        try
        {
            return Ok(subscriptionService.GetSubscriptors(id));
        }
        catch (ResearcherNotFoundException ex)
        {
            _logger.LogError(ex, "Researcher not found.");
            return NotFound("Researcher not found.");
        }
        catch (SubscriptionServiceException ex)
        {
            _logger.LogError(ex, "Error while retrieving subscriptors.");
            return StatusCode(500, "An error occurred while retrieving subscriptors.");
        }
    }

    /// <summary>
    /// Gets all the subscriptions of a specific researcher.
    /// </summary>
    /// <param name="id">The researcher ID.</param>
    /// <returns>A list of subscriptions.</returns>
    [HttpGet("subscriptions/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Subscription>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetSubscriptions(Guid id)
    {
        try
        {
            return Ok(subscriptionService.GetSubscriptions(id));
        }
        catch (ResearcherNotFoundException ex)
        {
            _logger.LogError(ex, "Researcher not found.");
            return NotFound("Researcher not found.");
        }
        catch (SubscriptionServiceException ex)
        {
            _logger.LogError(ex, "Error while retrieving subscriptions.");
            return StatusCode(500, "An error occurred while retrieving subscriptions.");
        }
    }

    /// <summary>
    /// Gets the feed of journals from researchers the specified researcher is subscribed to.
    /// </summary>
    /// <param name="id">The researcher ID.</param>
    /// <returns>The feed of journals.</returns>
    [HttpGet("feed/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Journal>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetFeed(Guid id)
    {
        try
        {
            return Ok(subscriptionService.GetFeed(id));
        }
        catch (ResearcherNotFoundException ex)
        {
            _logger.LogError(ex, "Researcher not found.");
            return NotFound("Researcher not found.");
        }
        catch (SubscriptionServiceException ex)
        {
            _logger.LogError(ex, "Error while retrieving feed.");
            return StatusCode(500, "An error occurred while retrieving feed.");
        }
    }

    /// <summary>
    /// Creates a new subscription between researchers.
    /// </summary>
    /// <param name="subscription">The subscription object to create.</param>
    /// <returns>The newly created subscription.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Subscription), 200)]
    [ProducesResponseType(500)]
    public IActionResult Post([FromBody] Subscription subscription)
    {
        try
        {
            subscriptionService.Create(subscription);
            return Ok(
                CreatedAtAction(
                    nameof(Post),
                    new { id = subscription.SubscriptionId },
                    subscription
                )
            );
        }
        catch (SubscriptionServiceException ex)
        {
            _logger.LogError(ex, "Error while creating subscription.");
            return StatusCode(500, "An error occurred while creating the subscription.");
        }
    }

    /// <summary>
    /// Deletes a specific subscription.
    /// </summary>
    /// <param name="id">The subscription ID to delete.</param>
    /// <returns>A status code indicating the result of the deletion.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            bool deleteResult = await subscriptionService.Delete(id);
            if (deleteResult)
            {
                return Ok();
            }
            else
            {
                _logger.LogError("Subscription not found.");
                return NotFound("Subscription not found.");
            }
        }
        catch (SubscriptionServiceException ex)
        {
            _logger.LogError(ex, "Error while deleting subscriptors.");
            return StatusCode(500, "An error occurred while deleting subscriptors.");
        }
    }
}
