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
            return context.Journals.Where<Journal>(journal => journal.ResearcherId == researcherId);
        }
        catch (Exception)
        {
            throw new JournalServiceException("Error while getting journals");
        }
    }

    /// <summary>
    /// Creates a new journal.
    /// </summary>
    /// <param name="journal">The journal information to create.</param>
    public async Task Create(Journal journal)
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
        }
        catch (Exception)
        {
            throw new JournalServiceException("Error while creating journal");
        }
    }

    /// <summary>
    /// Updates an existing journal.
    /// </summary>
    /// <param name="id">The ID of the journal to update.</param>
    /// <param name="journal">The updated journal information.</param>
    public async Task Update(Guid id, Journal journal)
    {
        try
        {
            var currentJournal = context.Journals.Find(id);
            if (currentJournal == null)
            {
                throw new JournalNotFoundException("Journal not found");
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
    /// Deletes a journal by its ID.
    /// </summary>
    /// <param name="id">The ID of the journal to delete.</param>
    public async Task Delete(Guid id)
    {
        try
        {
            var currentJournal = context.Journals.Find(id);
            if (currentJournal == null)
            {
                throw new JournalNotFoundException("Journal not found");
            }
            else
            {
                removeFile(currentJournal.InternalUrl);
                context.Journals.Remove(currentJournal);
                await context.SaveChangesAsync();
            }
        }
        catch (JournalNotFoundException)
        {
            throw;
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

    Task Create(Journal journal);

    Task Update(Guid id, Journal journal);

    Task Delete(Guid id);
}
