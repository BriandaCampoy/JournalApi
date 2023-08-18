using journalapi.Models;

namespace journalapi.Services;

/// <summary>
/// Service to handle operations related to Researchers.
/// </summary>
public class ReasearcherService : IReasearcherService
{
    protected readonly JournalContext context;

    protected readonly EncryptService encryptService;

    /// <summary>
    /// Initializes a new instance of the ReasearcherService class.
    /// </summary>
    /// <param name="dbcontext">The JournalContext instance.</param>
    public ReasearcherService(JournalContext dbcontext, EncryptService encryptService)
    {
        context = dbcontext;
        this.encryptService = encryptService;
    }

    /// <summary>
    /// Gets all the researchers.
    /// </summary>
    /// <returns>List of researchers.</returns>
    public IEnumerable<Researcher> Get()
    {
        try
        {
            return context.Researchers;
        }
        catch (Exception ex)
        {
            throw new ResearcherServiceException("Error while getting researchers", ex);
        }
    }

    /// <summary>
    /// Gets a researcher by their ID.
    /// </summary>
    /// <param name="researcherId">The ID of the researcher to retrieve.</param>
    /// <returns>The researcher with the given ID.</returns>
    public Researcher GetOne(Guid researcherId)
    {
        try
        {
            var researcher = context.Researchers.Find(researcherId);
            if (researcher == null)
            {
                throw new ResearcherNotFoundException("Researcher not found");
            }
            return researcher;
        }
        catch (ResearcherNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ResearcherServiceException("Error while getting researcher", ex);
        }
    }

    /// <summary>
    /// Creates a new researcher.
    /// </summary>
    /// <param name="researcher">The researcher to be created.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result contains:
    /// - If successful: An IResult with a "201 Created" status code and the created researcher.
    /// - If an error occurs: Throws a ResearcherServiceException with details about the error.
    /// </returns>
    /// <exception cref="ResearcherServiceException">Thrown when an error occurs while creating the researcher.</exception>

    public async Task<IResult> Create(Researcher researcher)
    {
        try
        {
            researcher.ResearcherId = Guid.NewGuid();
            researcher.Password = encryptService.Encrypt(researcher.Password);
            context.Researchers.Add(researcher);
            await context.SaveChangesAsync();
            return Results.Created("Created", researcher);
        }
        catch (Exception ex)
        {
            throw new ResearcherServiceException("Error while creating researcher", ex);
        }
    }

    /// <summary>
    /// Updates an existing researcher's information.
    /// </summary>
    /// <param name="id">The unique identifier of the researcher to update.</param>
    /// <param name="researcher">The updated information for the researcher.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result contains:
    /// - If the researcher is found and updated: An IResult with a "200 OK" status code.
    /// - If the researcher is not found: An IResult with a "404 Not Found" status code.
    /// - If an error occurs: Throws a ResearcherServiceException with details about the error.
    /// </returns>
    /// <exception cref="ResearcherServiceException">Thrown when an error occurs while updating the researcher.</exception>
    public async Task<IResult> Update(Guid id, Researcher researcher)
    {
        try
        {
            var currentResearcher = context.Researchers.Find(id);
            if (currentResearcher == null)
            {
                return Results.NotFound();
            }
            else
            {
                currentResearcher.Name = researcher.Name;
                currentResearcher.Password = encryptService.Encrypt(researcher.Password);

                await context.SaveChangesAsync();
                return Results.Ok();
            }
        }
        catch (Exception ex)
        {
            throw new ResearcherServiceException("Error while updating researcher", ex);
        }
    }

    /// <summary>
    /// Deletes an existing researcher by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the researcher to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result contains:
    /// - If the researcher is found and deleted: An IResult with a "200 OK" status code.
    /// - If the researcher is not found: An IResult with a "404 Not Found" status code.
    /// - If an error occurs: Throws a ResearcherServiceException with details about the error.
    /// </returns>
    /// <exception cref="ResearcherServiceException">Thrown when an error occurs while deleting the researcher.</exception>
    public async Task<IResult> Delete(Guid id)
    {
        try
        {
            var currentResearcher = context.Researchers.Find(id);
            if (currentResearcher == null)
            {
                return Results.NotFound();
            }
            else
            {
                context.Researchers.Remove(currentResearcher);
                await context.SaveChangesAsync();
                return Results.Ok();
            }
        }
        catch (Exception ex)
        {
            throw new ResearcherServiceException("Error while deleting researcher", ex);
        }
    }

}

public interface IReasearcherService
{
    IEnumerable<Researcher> Get();
    Researcher GetOne(Guid researcherId);
    Task<IResult> Create(Researcher researcher);
    Task<IResult> Update(Guid id, Researcher researcher);
    Task<IResult> Delete(Guid id);
}


