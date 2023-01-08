using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DocumentManager.Context;

public class DocumentManagerContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<GuestUser> GuestUsers { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Folder> Folders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=127.0.0.1,1433;
                        Initial Catalog=DocumentManagerDB;
                        User Id=sa;
                        Password=SqlServer2019;
                        Encrypt=False;
                        Trusted_Connection=True;
                        TrustServerCertificate=True;
                        Integrated Security=False;", 
            options => options.EnableRetryOnFailure());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentTag>()
            .HasKey(dt => new { dt.DocumentId, dt.TagId });

        modelBuilder.Entity<DocumentTag>()
            .HasOne(dt => dt.Document)
            .WithMany(d => d.DocumentTags)
            .HasForeignKey(dt => dt.DocumentId);

        modelBuilder.Entity<DocumentTag>()
            .HasOne(dt => dt.Tag)
            .WithMany(t => t.DocumentTags)
            .HasForeignKey(dt => dt.TagId);

        modelBuilder.Entity<AggregateRoot>()
            .Property(p => p.ManagedItems)
            .HasConversion(new ValueConverter<ICollection<object>, string>(
                v => string.Join(',', v.Select(x => x.ToString())),
                v => v.Split(',', StringSplitOptions.None).Select(x => (object)Convert.ChangeType(x, typeof(object))).ToList()));
    }
}