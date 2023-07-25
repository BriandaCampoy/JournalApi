using Microsoft.EntityFrameworkCore;
using journalapi.Models;

namespace journalapi;

public class JournalContext : DbContext{

  public DbSet<Researcher> Researchers { get; set; }

  public DbSet<Journal> Journals { get; set; }

  public DbSet<Subscription> Subscriptions { get; set; }

  public JournalContext(DbContextOptions<JournalContext> options):base(options){ }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Researcher>(researcher=>{
      researcher.ToTable("Researcher");
      researcher.HasKey(p=>p.ResearcherId);
      researcher.Property(p=>p.Name).IsRequired().HasMaxLength(150);
      researcher.Property(p=>p.Password).IsRequired();
      researcher.Property(p=>p.Name).IsRequired();
    });

    modelBuilder.Entity<Journal>(journal=>{
      journal.ToTable("Journal");
      journal.HasKey(p=>p.JournalId);
      journal.HasOne(p=>p.Researcher).WithMany(p=>p.Journals).HasForeignKey(p=>p.ResearcherId);
      journal.Property(p=>p.Title).IsRequired().HasMaxLength(200);
      journal.Property(p=>p.InternalUrl).IsRequired();
      journal.Property(p=>p.Url).IsRequired();
      journal.Property(p=>p.PublishedDate);
      journal.Ignore(p=>p.journalFile);
    });

    modelBuilder.Entity<Subscription>(subscription=>{
      subscription.ToTable("Subscription");
      subscription.HasKey(p=>p.SubscriptionId);
      subscription.HasOne(p=>p.researcher).WithMany(p=>p.Subscriptions).HasForeignKey(p=>p.ResearcherId).OnDelete(DeleteBehavior.NoAction);
      subscription.HasOne(p=>p.FollowedResearcher).WithMany(p=>p.Subscriptors).HasForeignKey(p=>p.FollowedResearcherId).OnDelete(DeleteBehavior.NoAction);;
    });
  }


}