using journalapi.Models;
using journalApi.Services;

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
    public async Task<IResult> Create(Researcher researcher)
    {
        try
        {
            researcher.ResearcherId = Guid.NewGuid();

            byte[] encData_byte = new byte[researcher.Password.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(researcher.Password);
            researcher.Password = Convert.ToBase64String(encData_byte);

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
    /// Updates an existing researcher.
    /// </summary>
    /// <param name="id">The ID of the researcher to update.</param>
    /// <param name="researcher">The updated researcher data.</param>
    public async Task<bool> Update(Guid id, Researcher researcher)
    {
        try
        {
            var currentResearcher = context.Researchers.Find(id);
            if (currentResearcher == null)
            {
                return false;
            }
            else
            {
                currentResearcher.Name = researcher.Name;
                currentResearcher.Password = researcher.Password;
                currentResearcher.Email = researcher.Email;

                await context.SaveChangesAsync();
                return true;
            }
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
    public async Task<bool> Delete(Guid id)
    {
        try
        {
            var currentResearcher = context.Researchers.Find(id);
            if (currentResearcher == null)
            {
                return false;
            }
            else
            {
                context.Researchers.Remove(currentResearcher);
                await context.SaveChangesAsync();
                return true;
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
    Task<bool> Update(Guid id, Researcher researcher);
    Task<bool> Delete(Guid id);
}
