using Microsoft.AspNetCore.Mvc;
using journalapi.Models;
using journalapi.Services;

namespace journalapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResearcherController : ControllerBase
{
    IReasearcherService researcherService;
    protected readonly JournalContext context;
    private readonly ILogger<ResearcherController> _logger;

    public ResearcherController(
        JournalContext dbContext,
        IReasearcherService service,
        ILogger<ResearcherController> logger
    )
    {
        _logger = logger;
        researcherService = service;
        context = dbContext;
    }

    [HttpGet]
    [Route("createdb")]
    public IActionResult CreateDatabase()
    {
        context.Database.EnsureCreated();
        return Ok();
    }

    /// <summary>
    /// Gets all the researchers.
    /// </summary>
    /// <returns>ListResearcher</returns>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(researcherService.Get());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id){
        return Ok(researcherService.GetOne(id));
    }


    [HttpPost]
    public IActionResult Post([FromBody] Researcher researcher)
    {
        try
        {
            researcherService.Create(researcher);
            return Ok();
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, [FromBody] Researcher researcher)
    {
        researcherService.Update(id, researcher);
        return Ok();
    }

    /// <summary>
    /// Deletes a specific Researcher.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        researcherService.Delete(id);
        return Ok();
    }
}
