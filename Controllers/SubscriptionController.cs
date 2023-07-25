using Microsoft.AspNetCore.Mvc;
using journalapi.Models;
using journalapi.Services;

namespace journalapi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SubscriptionController : ControllerBase{

  private readonly ILogger<SubscriptionController> _logger;

  protected readonly ISubscriptionService subscriptionService;

  public SubscriptionController(ISubscriptionService service, ILogger<SubscriptionController> logger){
    subscriptionService = service;
    _logger = logger;
  }

  [HttpGet("subscriptors/{id}")]

  public IActionResult GetSubscriptors(Guid id){
    return Ok(subscriptionService.GetSubscriptors(id));
  }

  [HttpGet("subscriptions/{id}")]
  public IActionResult GetSubscriptions(Guid id){
    return Ok(subscriptionService.GetSubscriptions(id));
  }

  [HttpGet("feed/{id}")]
  public IActionResult GetFeed(Guid id){
    return Ok(subscriptionService.GetFeed(id));
  }

  [HttpPost]
  public IActionResult Post([FromBody]Subscription subscription){
    subscriptionService.Create(subscription);
    return Ok(CreatedAtAction(nameof(Post), new { id = subscription.SubscriptionId }, subscription));
  }

  [HttpDelete("{id}")]
  public IActionResult Delete(Guid id){
    subscriptionService.Delete(id);
    return Ok();
  }


}