using Microsoft.AspNetCore.Mvc;
using journalapi.Models;
using journalapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace journalapi.Controllers;

/// <summary>
/// Controller to handle operations related to Researchers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    /// Deletes researchers who do not have associated journals.
    /// </summary>
    /// <remarks>
    /// This endpoint triggers a stored procedure named "DeleteResearchersWithoutJournals" to remove researchers who do not have any journals associated with them.
    /// </remarks>
    /// <returns>Status code indicating the result of the operation.</returns>
    [HttpDelete]
    [Route("cleanResearchers")]
    public IActionResult DeleteEmptyResearchers()
    {
        try
        {
        context.Database.ExecuteSqlRaw("EXEC DeleteResearchersWithoutJournals");
        return Ok();

        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting empty researchers.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting empty researchers.");
        }
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
    /// Updates an existing researcher.
    /// </summary>
    /// <param name="id">The ID of the researcher to update.</param>
    /// <param name="researcher">The updated researcher data.</param>
    /// <returns>200 OK if successful.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Put(Guid id, [FromBody] Researcher researcher)
    {
        try
        {
            bool updateResult = await researcherService.Update(id, researcher);
            if (updateResult)
            {
                return Ok();
            }
            else
            {
                _logger.LogError("Researcher not found.");
                return NotFound("Researcher not found.");
            }
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
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            bool deleteResult = await researcherService.Delete(id);
            if (deleteResult)
            {
                return Ok();
            }
            else
            {
                _logger.LogError("Researcher not found.");
                return NotFound("Researcher not found.");
            }
        }
        catch (ResearcherServiceException ex)
        {
            _logger.LogError(ex, "Error while deleting researcher.");
            return StatusCode(500, "An error occurred while deleting researcher.");
        }
    }
}
