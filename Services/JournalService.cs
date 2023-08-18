using journalapi.Models;

namespace journalapi.Services;

/// <summary>
/// Service responsible for managing journals.
/// </summary>
public class JournalService : IJournalService
{
    protected readonly JournalContext context;

    private readonly IWebHostEnvironment _webHostEnvironment;

    /// <summary>
    /// Initializes a new instance of the <see cref="JournalService"/> class.
    /// </summary>
    /// <param name="dbcontext">The database context.</param>
    /// <param name="webHost">The web host environment.</param>
    public JournalService(JournalContext dbcontext, IWebHostEnvironment webHost)
    {
        _webHostEnvironment = webHost;
        context = dbcontext;
    }

    /// <summary>
    /// Retrieves all journals.
    /// </summary>
    /// <returns>A list of journals.</returns>
    public IEnumerable<Journal> Get()
    {
        try
        {
            return context.Journals;
        }
        catch (Exception)
        {
            throw new JournalServiceException("Error while getting journals");
        }
    }

    /// <summary>
    /// Retrieves a specific journal by its ID.
    /// </summary>
    /// <param name="journalId">The ID of the journal to retrieve.</param>
    /// <returns>The requested journal.</returns>
    public Journal GetOne(Guid journalId)
    {
        try
        {
            var journal = context.Journals.Find(journalId);
            if (journal == null)
            {
                throw new JournalNotFoundException("Journal not found");
            }
            else
            {
                return journal;
            }
        }
        catch (JournalNotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new JournalServiceException("Error while getting journal");
        }
    }

    /// <summary>
    /// Retrieves all journals for a specific researcher.
    /// </summary>
    /// <param name="researcherId">The ID of the researcher.</param>
    /// <returns>A list of journals for the researcher.</returns>
    public IEnumerable<Journal> GetByResearcher(Guid researcherId)
    {
        try
        {
            var researcher = context.Researchers.Find(researcherId);
            if (researcher == null)
            {
                throw new ResearcherNotFoundException("Researcher not found");
            }
            return context.Journals.Where<Journal>(journal => journal.ResearcherId == researcherId);
        }
        catch (ResearcherNotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new JournalServiceException("Error while getting journals");
        }
    }

    /// <summary>
    /// Creates a new journal and stores it in the database.
    /// </summary>
    /// <param name="journal">The journal object to create.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result contains:
    /// - If the journal is successfully created: An IResult with a "201 Created" status code and the created journal.
    /// - If an error occurs while creating the journal: Throws a JournalServiceException with details about the error.
    /// </returns>
    /// <exception cref="JournalServiceException">Thrown when an error occurs while creating the journal.</exception>
    public async Task<IResult> Create(Journal journal)
    {
        try
        {
            string pathNewFile = generateFile(journal.journalFile);
            journal.InternalUrl = pathNewFile;
            journal.JournalId = Guid.NewGuid();
            journal.Url = "http://localhost:5155/api/Journal/docFile/" + journal.JournalId;
            journal.PublishedDate = DateTime.Now;
            context.Journals.Add(journal);
            await context.SaveChangesAsync();
                return Results.Created("created", journal);
        }
        catch (Exception)
        {
            throw new JournalServiceException("Error while creating journal");
        }
    }

    /// <summary>
    /// Updates an existing journal in the database.
    /// </summary>
    /// <param name="id">The unique identifier of the journal to update.</param>
    /// <param name="journal">The updated journal object.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result indicates:
    /// - If the journal is successfully updated: True.
    /// - If the journal does not exist: False.
    /// - If an error occurs while updating the journal: Throws a JournalServiceException with details about the error.
    /// </returns>
    /// <exception cref="JournalNotFoundException">Thrown when the journal with the given ID is not found.</exception>
    /// <exception cref="JournalServiceException">Thrown when an error occurs while updating the journal.</exception>
    public async Task<bool> Update(Guid id, Journal journal)
    {
        try
        {
            var currentJournal = context.Journals.Find(id);
            if (currentJournal == null)
            {
                return false;
            }
            else
            {
                currentJournal.Title = journal.Title;
                if (journal.journalFile != null)
                {
                    removeFile(currentJournal.InternalUrl);
                    string pathNewFile = generateFile(journal.journalFile);
                    currentJournal.InternalUrl = pathNewFile;
                    currentJournal.Url =
                        "http://localhost:5155/api/Journal/docFile/" + currentJournal.JournalId;
                }

                await context.SaveChangesAsync();
                return true;
            }
        }
        catch (JournalNotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new JournalServiceException("Error while updating journal");
        }
    }

    /// <summary>
    /// Deletes an existing journal from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the journal to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result indicates:
    /// - If the journal is successfully deleted: True.
    /// - If the journal does not exist: False.
    /// - If an error occurs while deleting the journal: Throws a JournalServiceException with details about the error.
    /// </returns>
    /// <exception cref="JournalServiceException">Thrown when an error occurs while deleting the journal.</exception>
    public async Task<bool> Delete(Guid id)
    {
        try
        {
            var currentJournal = context.Journals.Find(id);
            if (currentJournal == null)
            {
               return false;
            }
            else
            {
                removeFile(currentJournal.InternalUrl);
                context.Journals.Remove(currentJournal);
                await context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception)
        {
            throw new JournalServiceException("Error while creating journal");
        }
    }

    private void removeFile(string internalUrl)
    {
        try
        {
            if (File.Exists(internalUrl))
            {
                File.Delete(internalUrl);
            }
        }
        catch (Exception)
        {
            throw new JournalServiceException("Error while removing file");
        }
    }

    private string generateFile(IFormFile journalFile)
    {
        try
        {
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Files");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string randomFileName = Path.GetRandomFileName();
            string fileExtension = Path.GetExtension(journalFile.FileName);
            string fullFileName = randomFileName + fileExtension;

            string fullPath = Path.Combine(path, fullFileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                journalFile.CopyTo(stream);
            }
            return fullPath;
        }
        catch (Exception)
        {
            throw new JournalServiceException("Error while generating file");
        }
    }
}

public interface IJournalService
{
    IEnumerable<Journal> Get();
    Journal GetOne(Guid journalId);
    IEnumerable<Journal> GetByResearcher(Guid researcherId);

    Task<IResult> Create(Journal journal);

    Task<bool> Update(Guid id, Journal journal);

    Task<bool> Delete(Guid id);
}
