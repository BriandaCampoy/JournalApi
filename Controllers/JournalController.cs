using Microsoft.AspNetCore.Mvc;
using journalapi.Models;
using journalapi.Services;

namespace journalapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JournalController : ControllerBase
{

    private readonly ILogger<JournalController> _logger;

    protected readonly IJournalService journalService;

    public JournalController(IJournalService service, ILogger<JournalController> logger)
    {

        journalService = service;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(journalService.Get());
    }

    [HttpGet("{id}")]
    public IActionResult GetByJournalId(Guid id)
    {
        return Ok(journalService.GetOne(id));
    }

    [HttpGet("researcher/{id}")]
    public IActionResult GetJournalByResearcherId(Guid id)
    {
        return Ok(journalService.GetByResearcher(id));
    }

    [HttpGet("docFile/{idJournal}")]
    public IActionResult GetJournalDoc(Guid idJournal){
      string route = journalService.GetOne(idJournal).InternalUrl;
      if(!System.IO.File.Exists(route)){
        return NotFound();
      }
      byte[] file = System.IO.File.ReadAllBytes(route);

      return File(file, "application/pdf", route);
    }

    [HttpPost]
    public IActionResult Post([FromForm] Journal journal)
    {
      try
      {
        if(journal.journalFile.Length ==0){return BadRequest();}
        journalService.Create(journal);
        return Ok();
      }
      catch (Exception)
      {
        return BadRequest();
      }
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, [FromForm] Journal journal)
    {
        journalService.Update(id, journal);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        journalService.Delete(id);
        return Ok();
    }
}
