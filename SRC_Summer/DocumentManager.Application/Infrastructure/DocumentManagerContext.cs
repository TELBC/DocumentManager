using DocumentManager.Model;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Infrastructure;

public class DocumentManagerContext : DbContext
{
    public DocumentManagerContext(DbContextOptions opt) : base(opt)
    {
    }

    public DbSet<User> User => Set<User>();
    public DbSet<UserBase> UserBase => Set<UserBase>();
    public DbSet<Tag> Tag => Set<Tag>();
    public DbSet<Document> Document => Set<Document>();
    public DbSet<Folder> Folder => Set<Folder>();

    public DbSet<Model.DocumentManager> DocumentManager => Set<Model.DocumentManager>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // value converter
        modelBuilder
            .Entity<Tag>()
            .Property(c => c.Category)
            .HasConversion<String>();
        modelBuilder.Entity<User>().ToTable("User");
    }
}