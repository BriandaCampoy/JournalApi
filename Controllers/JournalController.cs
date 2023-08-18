using Microsoft.AspNetCore.Mvc;
using journalapi.Models;
using journalapi.Services;
using Microsoft.AspNetCore.Authorization;

namespace journalapi.Controllers;

/// <summary>
/// API endpoints for managing journals.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JournalController : ControllerBase
{
    private readonly ILogger<JournalController> _logger;

    protected readonly IJournalService journalService;

    /// <summary>
    /// Initializes a new instance of the <see cref="JournalController"/> class.
    /// </summary>
    /// <param name="service">The journal service.</param>
    /// <param name="logger">The logger.</param>
    public JournalController(IJournalService service, ILogger<JournalController> logger)
    {
        journalService = service;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all journals.
    /// </summary>
    /// <returns>A list of journals.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Journal>), 200)]
    [ProducesResponseType(500)]
    public IActionResult Get()
    {
        try
        {
            return Ok(journalService.Get());
        }
        catch (JournalServiceException ex)
        {
            _logger.LogError(ex, "Error while consulting journals.");
            return StatusCode(500, "An error while consulting journals.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while consulting journals.");
            return StatusCode(500, "An error while consulting journals.");
        }
    }

    /// <summary>
    /// Retrieves a specific journal by its ID.
    /// </summary>
    /// <param name="id">The ID of the journal to retrieve.</param>
    /// <returns>The requested journal.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Journal), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetByJournalId(Guid id)
    {
        try
        {
            return Ok(journalService.GetOne(id));
        }
        catch (JournalNotFoundException ex)
        {
            _logger.LogError(ex, "Journal not found.");
            return NotFound("Journal not found.");
        }
        catch (JournalServiceException ex)
        {
            _logger.LogError(ex, "Error while consulting journal.");
            return StatusCode(500, "An error while consulting journal.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while consulting journal.");
            return StatusCode(500, "An error while consulting journal.");
        }
    }

    /// <summary>
    /// Retrieves all journals for a specific researcher.
    /// </summary>
    /// <param name="id">The ID of the researcher.</param>
    /// <returns>A list of journals for the researcher.</returns>
    [HttpGet("researcher/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Journal>), 200)]
    [ProducesResponseType(500)]
    public IActionResult GetJournalByResearcherId(Guid id)
    {
        try
        {
            return Ok(journalService.GetByResearcher(id));
        }
        catch (ResearcherNotFoundException ex)
        {
            _logger.LogError(ex, "Researcher not found.");
            return NotFound("Researcher not found.");
        }
        catch (JournalServiceException)
        {
            return StatusCode(500, "An error while consulting journals.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while consulting journals.");
            return StatusCode(500, "An error while consulting journals.");
        }
    }

    /// <summary>
    /// Retrieves the PDF document of a specific journal.
    /// </summary>
    /// <param name="idJournal">The ID of the journal.</param>
    /// <returns>The PDF file of the journal.</returns>
    [HttpGet("docFile/{idJournal}")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetJournalDoc(Guid idJournal)
    {
        try
        {
            string route = journalService.GetOne(idJournal).InternalUrl;
            if (!System.IO.File.Exists(route))
            {
                return NotFound("File not found");
            }
            byte[] file = System.IO.File.ReadAllBytes(route);

            return File(file, "application/pdf", route);
        }
        catch (JournalNotFoundException)
        {
            return NotFound("Journal file not found");
        }
        catch (JournalServiceException)
        {
            return StatusCode(500, "An error while consulting file.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while consulting file.");
            return StatusCode(500, "An error while consulting file.");
        }
    }

    /// <summary>
    /// Creates a new journal.
    /// </summary>
    /// <param name="journal">The journal information to create.</param>
    /// <returns>Ok if the journal was created successfully.</returns>
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult>Post([FromForm] Journal journal)
    {
        try
        {
            if (journal.journalFile == null || journal.journalFile.Length == 0)
            {
                return BadRequest("Journal file cannot be empty.");
            }
            var result = await journalService.Create(journal);
            return Ok(result);
        }
        catch (JournalServiceException)
        {
            return StatusCode(500, "An error while creating the journal");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating journal.");
            return StatusCode(500, "An error occurred while creating the journal.");
        }
    }

    /// <summary>
    /// Updates an existing journal.
    /// </summary>
    /// <param name="id">The ID of the journal to update.</param>
    /// <param name="journal">The updated journal information.</param>
    /// <returns>Ok if the journal was updated successfully.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Put(Guid id, [FromForm] Journal journal)
    {
        try
        {
            bool updateResult = await journalService.Update(id, journal);
            if (updateResult)
            {
                return Ok();
            }
            else
            {
                _logger.LogError("Journal not founded.");
                return NotFound("Journal not found here.");
            }
        }
        catch (JournalServiceException)
        {
            return StatusCode(500, "An error occurred while updating the journal.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating journal.");
            return StatusCode(500, "An error occurred while updating the journal.");
        }
    }

    /// <summary>
    /// Deletes a journal by its ID.
    /// </summary>
    /// <param name="id">The ID of the journal to delete.</param>
    /// <returns>Ok if the journal was deleted successfully.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            bool deleteResult = await journalService.Delete(id);
            if (deleteResult)
            {
                return Ok();
            }
            else
            {
                _logger.LogError("Journal not found.");
                return NotFound("Journal not found.");
            }
        }
        catch (JournalServiceException)
        {
            return StatusCode(500, "An error occurred while deleting the journal.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting journal.");
            return StatusCode(500, "An error occurred while deleting the journal.");
        }
    }
}
