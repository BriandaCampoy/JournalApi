using journalapi.Models;

namespace journalapi.Services;

/// <summary>
/// Service to handle operations related to Researchers.
/// </summary>
public class ReasearcherService : IReasearcherService
{
    protected readonly JournalContext context;

    /// <summary>
    /// Initializes a new instance of the ReasearcherService class.
    /// </summary>
    /// <param name="dbcontext">The JournalContext instance.</param>
    public ReasearcherService(JournalContext dbcontext)
    {
        context = dbcontext;
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
    /// <param name="researcher">The researcher to create.</param>
    public async Task Create(Researcher researcher)
    {
        try
        {
            researcher.ResearcherId = Guid.NewGuid();
            context.Researchers.Add(researcher);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new ResearcherServiceException("Error while creating researcher", ex);
        }
    }

    /// <summary>
    /// Updates an existing researcher.
    /// </summary>
    /// <param name="id">The ID of the researcher to update.</param>
    /// <param name="researcher">The updated researcher data.</param>
    public async Task Update(Guid id, Researcher researcher)
    {
        try
        {
            var currentResearcher = context.Researchers.Find(id);
            if (currentResearcher == null)
            {
                throw new ResearcherNotFoundException("Researcher not found");
            }
            else
            {
                currentResearcher.Name = researcher.Name;
                currentResearcher.Password = researcher.Password;
                currentResearcher.Email = researcher.Email;

                await context.SaveChangesAsync();
            }
        }
        catch (ResearcherNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ResearcherServiceException("Error while updating researcher", ex);
        }
    }

    /// <summary>
    /// Deletes a specific researcher.
    /// </summary>
    /// <param name="id">The ID of the researcher to delete.</param>
    public async Task Delete(Guid id)
    {
        try
        {
            var currentResearcher = context.Researchers.Find(id);
            if (currentResearcher == null)
            {
                throw new ResearcherNotFoundException("Researcher not found");
            }
            else
            {
                context.Researchers.Remove(currentResearcher);
                await context.SaveChangesAsync();
            }
        }
        catch (ResearcherNotFoundException)
        {
            throw;
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
    Task Create(Researcher researcher);
    Task Update(Guid id, Researcher researcher);
    Task Delete(Guid id);
}
