using DocumentManager.Model;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Infrastructure;

public class DocumentManagerContext : DbContext
{
    
    public DocumentManagerContext(DbContextOptions opt) :base(opt){}
    public DbSet<User> Users => Set<User>();
    public DbSet<GuestUser> GuestUsers => Set<GuestUser>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Folder> Folders => Set<Folder>();

    public DbSet<Model.DocumentManager> DocumentManager => Set<Model.DocumentManager>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // value converter
        modelBuilder
            .Entity<Tag>()
            .Property(c => c.Category)
            .HasConversion(
            c => c.ToString(),
            c => (Category) Enum.Parse(typeof(Category), c)
        );
        modelBuilder.Entity<Model.DocumentManager>()
            .HasKey(dm => dm.Id);

        // modelBuilder.Entity<Model.DocumentManager>()
        //     .HasMany(dm => dm.Folders);
        // modelBuilder.Entity<Document>().HasMany<Tag>(d => d.Tags);
        // modelBuilder.Entity<Document>().HasOne<Folder>(d => d.Folder);
        // modelBuilder.Entity<Folder>().OwnsMany<Document>(f => f.Documents);
    }
}