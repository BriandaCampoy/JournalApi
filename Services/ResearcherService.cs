using journalapi.Models;

namespace journalapi.Services;

public class ReasearcherService:IReasearcherService{
  protected readonly JournalContext context;

  public ReasearcherService(JournalContext dbcontext){
    context = dbcontext;
  }

  public IEnumerable<Researcher>Get(){
    return context.Researchers;
  }

  public Researcher GetOne(Guid researcherId){
    var researcher = context.Researchers.Find(researcherId);
    return researcher;
  }

  public async Task Create(Researcher researcher){
    researcher.ResearcherId =Guid.NewGuid();
    context.Researchers.Add(researcher);
    await context.SaveChangesAsync();
  }

  public async Task Update(Guid id, Researcher researcher){
    var currentResearcher = context.Researchers.Find(id);
    if(currentResearcher!=null){
      currentResearcher.Name = researcher.Name;
      currentResearcher.Password = researcher.Password;
      currentResearcher.Email = researcher.Email;

      await context.SaveChangesAsync();
    }
  }

  public async Task Delete(Guid id){
    var currentResearcher = context.Researchers.Find(id);
    if(currentResearcher!=null){
      context.Researchers.Remove(currentResearcher);
      await context.SaveChangesAsync();
    }

  }


}


public interface IReasearcherService{
  IEnumerable<Researcher>Get();
  Researcher GetOne(Guid researcherId);
  Task Create(Researcher researcher);
  Task Update(Guid id, Researcher researcher);
  Task Delete(Guid id);

}