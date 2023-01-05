using DocumentManager;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Context;

public class DocumentManagerContext : DbContext
{
    public DocumentManagerContext(DbContextOptions<DocumentManagerContext> options) : base(options)
    {
    }

    public DbSet<Document> Documents { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<GuestUser> GuestUsers { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<DocumentManager> DocumentManagers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentManager>().HasData(new
        {
            Id = 1,
            Users = new List<User>(),
            GuestUsers = new List<GuestUser>(),
            Folders = new List<Folder>()
        });
    }
}