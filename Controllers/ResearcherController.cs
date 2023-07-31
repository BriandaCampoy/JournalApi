using Microsoft.AspNetCore.Mvc;
using journalapi.Models;
using journalapi.Services;

namespace journalapi.Controllers;

/// <summary>
/// Controller to handle operations related to Researchers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ResearcherController : ControllerBase
{
    IReasearcherService researcherService;
    protected readonly JournalContext context;
    private readonly ILogger<ResearcherController> _logger;

    /// <summary>
    /// Initializes a new instance of the ResearcherController class.
    /// </summary>
    /// <param name="dbContext">The JournalContext instance.</param>
    /// <param name="service">The IReasearcherService instance.</param>
    /// <param name="logger">The ILogger instance.</param>
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

    /// <summary>
    /// Creates the database if it does not exist.
    /// </summary>
    /// <returns>200 OK if successful.</returns>
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
    /// <returns>List of researchers.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Researcher>), 200)]
    [ProducesResponseType(500)]
    public IActionResult Get()
    {
        try
        {
            return Ok(researcherService.Get());
        }
        catch (ResearcherServiceException ex)
        {
            _logger.LogError(ex, "Error while getting researchers.");
            return StatusCode(500, "An error occurred while getting researchers.");
        }
    }

    /// <summary>
    /// Gets a researcher by their ID.
    /// </summary>
    /// <param name="id">The ID of the researcher to retrieve.</param>
    /// <returns>The researcher with the given ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Journal), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetById(Guid id)
    {
        try
        {
            return Ok(researcherService.GetOne(id));
        }
        catch (ResearcherNotFoundException ex)
        {
            _logger.LogError(ex, "Researcher not found.");
            return NotFound("Researcher not found.");
        }
        catch (ResearcherServiceException ex)
        {
            _logger.LogError(ex, "Error while getting researcher.");
            return StatusCode(500, "An error occurred while getting researcher.");
        }
    }

    /// <summary>
    /// Creates a new researcher.
    /// </summary>
    /// <param name="researcher">The researcher to create.</param>
    /// <returns>200 OK if successful.</returns>
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public IActionResult Post([FromBody] Researcher researcher)
    {
        try
        {
            researcherService.Create(researcher);
            return Ok();
        }
        catch (ResearcherServiceException ex)
        {
            _logger.LogError(ex, "Error while creating researcher.");
            return StatusCode(500, "An error occurred while creating researcher.");
        }
    }

    /// <summary>
    /// Updates an existing researcher.
    /// </summary>
    /// <param name="id">The ID of the researcher to update.</param>
    /// <param name="researcher">The updated researcher data.</param>
    /// <returns>200 OK if successful.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult Put(Guid id, [FromBody] Researcher researcher)
    {
        try
        {
            researcherService.Update(id, researcher);
            return Ok();
        }
        catch (ResearcherNotFoundException ex)
        {
            _logger.LogError(ex, "Researcher not found.");
            return NotFound("Researcher not found.");
        }
        catch (ResearcherServiceException ex)
        {
            _logger.LogError(ex, "Error while updating researcher.");
            return StatusCode(500, "An error occurred while updating researcher.");
        }
    }

    /// <summary>
    /// Deletes a specific researcher.
    /// </summary>
    /// <param name="id">The ID of the researcher to delete.</param>
    /// <returns>200 OK if successful.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult Delete(Guid id)
    {
        try
        {
            researcherService.Delete(id);
            return Ok();
        }
        catch (ResearcherNotFoundException ex)
        {
            _logger.LogError(ex, "Researcher not found.");
            return NotFound("Researcher not found.");
        }
        catch (ResearcherServiceException ex)
        {
            _logger.LogError(ex, "Error while deleting researcher.");
            return StatusCode(500, "An error occurred while deleting researcher.");
        }
    }
}
