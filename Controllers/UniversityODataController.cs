using journalapi;
using journalapi.Models;
using journalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace journalApi.Controllers
{
    /// <summary>
    /// Controller to manage University data using OData.
    /// </summary>
    public class UniversityODataController : ODataController
    {
        private readonly JournalContext journalContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniversityODataController"/> class.
        /// </summary>
        /// <param name="context">The JournalContext instance.</param>
        public UniversityODataController(JournalContext context)
        {
            journalContext = context;
        }


        /// <summary>
        /// Retrieves a list of universities using OData query capabilities.
        /// </summary>
        /// <returns>A list of universities.</returns>
        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {

            return Ok(journalContext.Universities.AsQueryable());
        }


        /// <summary>
        /// Creates a new university.
        /// </summary>
        /// <param name="university">The university to create.</param>
        /// <returns>Created university.</returns>
        public async Task<IActionResult> Post([FromBody] University university)
        {
            if (university == null)
            {
                return BadRequest();
            }
            else
            {
                university.UniversityId = new Guid();
                journalContext.Add(university);
                await journalContext.SaveChangesAsync();
                return Created(university);
            }
        }

        /// <summary>
        /// Deletes a university by its key.
        /// </summary>
        /// <param name="key">The key of the university to delete.</param>
        /// <returns>Status code indicating the result of the operation.</returns>
        public async Task<IActionResult> Delete(Guid key)
        {
            var current = journalContext.Universities.FirstOrDefault(p => p.UniversityId == key);
            if (current == null)
            {
                return NotFound();
            }
            else
            {
                journalContext.Universities.Remove(current);
                await journalContext.SaveChangesAsync();
                return Ok();
            }
        }

        /// <summary>
        /// Updates an existing university by its key.
        /// </summary>
        /// <param name="key">The key of the university to update.</param>
        /// <param name="university">The updated university data.</param>
        /// <returns>Status code indicating the result of the operation.</returns>
        public async Task<IActionResult> Put(Guid key, [FromBody] University university)
        {
            var current = journalContext.Universities.FirstOrDefault(p=>p.UniversityId == key);
            if(current == null)
            {
                return NotFound();
            }
            else
            {
                current.nameUniversity = university.nameUniversity;
                current.city = university.city;
                await journalContext.SaveChangesAsync();
                return Updated(current);
            }
        }
    }
}
