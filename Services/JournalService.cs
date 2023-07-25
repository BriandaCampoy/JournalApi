using journalapi.Models;

namespace journalapi.Services;

public class JournalService : IJournalService
{
    protected readonly JournalContext context;

    private readonly IWebHostEnvironment _webHostEnvironment;

    public JournalService(JournalContext dbcontext, IWebHostEnvironment webHost)
    {
        _webHostEnvironment = webHost;
        context = dbcontext;
    }

    public IEnumerable<Journal> Get()
    {
        return context.Journals;
    }

    public Journal GetOne(Guid journalId)
    {
        var journal = context.Journals.Find(journalId);
        return journal;
    }

    public IEnumerable<Journal> GetByResearcher(Guid researcherId)
    {
        return context.Journals.Where<Journal>(journal => journal.ResearcherId == researcherId);
    }

    public async Task Create(Journal journal)
    {
        string pathNewFile = generateFile(journal.journalFile);
        journal.InternalUrl = pathNewFile;
        journal.JournalId = Guid.NewGuid();
        journal.Url = "http://localhost:5155/api/Journal/docFile/" + journal.JournalId;
        journal.PublishedDate = DateTime.Now;
        context.Journals.Add(journal);
        await context.SaveChangesAsync();
    }

    public async Task Update(Guid id, Journal journal)
    {
        var currentJournal = context.Journals.Find(id);
        if (currentJournal != null)
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

    public async Task Delete(Guid id)
    {
        var currentJournal = context.Journals.Find(id);
        if (currentJournal != null)
        {
            removeFile(currentJournal.InternalUrl);
            context.Journals.Remove(currentJournal);
            await context.SaveChangesAsync();
        }
    }

    private void removeFile(string internalUrl)
    {
        if (File.Exists(internalUrl))
        {
            File.Delete(internalUrl);
        }
    }

    private string generateFile(IFormFile journalFile)
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
