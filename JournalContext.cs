using Microsoft.EntityFrameworkCore;
using journalapi.Models;
using journalApi.Models;
using Microsoft.Identity.Client;

namespace journalapi;

/// <summary>
/// Represents the database context for the journal API.
/// </summary>
public class JournalContext : DbContext
{
    /// <summary>
    /// Gets or sets the DbSet for universities.
    /// </summary>
    public DbSet<University> Universities { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for researchers.
    /// </summary>
    public DbSet<Researcher> Researchers { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for journals.
    /// </summary>
    public DbSet<Journal> Journals { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for subscriptions.
    /// </summary>
    public DbSet<Subscription> Subscriptions { get; set; }

    /// <summary>
    /// Initializes a new instance of the JournalContext class with the specified options.
    /// </summary>
    /// <param name="options">The options to be used for configuring the context.</param>
    public JournalContext(DbContextOptions<JournalContext> options)
        : base(options) { }

    /// <summary>
    /// Configures the entity models and relationships for the database context.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<University>(university =>
        {
            university.ToTable("University");
            university.HasKey(p => p.UniversityId);
            university.Property(p=>p.nameUniversity).IsRequired();
            university.Property(p => p.city);
        });

        modelBuilder.Entity<Researcher>(researcher =>
        {
            researcher.ToTable("Researcher");
            researcher.HasKey(p => p.ResearcherId);
            researcher.Property(p => p.Name).IsRequired().HasMaxLength(150);
            researcher.Property(p => p.Password).IsRequired();
            researcher.Property(p => p.Name).IsRequired();
        });

        modelBuilder.Entity<Journal>(journal =>
        {
            journal.ToTable("Journal");
            journal.HasKey(p => p.JournalId);
            journal
                .HasOne(p => p.Researcher)
                .WithMany(p => p.Journals)
                .HasForeignKey(p => p.ResearcherId);
            journal.Property(p => p.Title).IsRequired().HasMaxLength(200);
            journal.Property(p => p.InternalUrl).IsRequired();
            journal.Property(p => p.Url).IsRequired();
            journal.Property(p => p.PublishedDate);
            journal.Ignore(p => p.journalFile);
        });

        modelBuilder.Entity<Subscription>(subscription =>
        {
            subscription.ToTable("Subscription");
            subscription.HasKey(p => p.SubscriptionId);
            subscription
                .HasOne(p => p.researcher)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(p => p.ResearcherId)
                .OnDelete(DeleteBehavior.NoAction);
            subscription
                .HasOne(p => p.FollowedResearcher)
                .WithMany(p => p.Subscriptors)
                .HasForeignKey(p => p.FollowedResearcherId)
                .OnDelete(DeleteBehavior.NoAction);
            ;
        });

    }
}
