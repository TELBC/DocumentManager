using System;
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
    
    public void CreateDatabase(bool isDevelopment)
    {
        if (isDevelopment) { Database.EnsureDeleted(); }
        if (Database.EnsureCreated()) { Initialize(); }
        if (isDevelopment) Seed();
    }
    
    private void Initialize()
    {
        // var author = new Author(
        //     firstname: "Max",
        //     lastname: "Mustermann",
        //     email: "mustermann@spengergasse.at",
        //     username: "admin",
        //     initialPassword: "1111",
        //     phone: "+4369912345678");
        // Authors.Add(author);
        // SaveChanges();
        //
        // var categories = new Category[]{
        //     new Category("Wissenschaft"),
        //     new Category("Kultur"),
        //     new Category("Sport")
        // };
        // Categories.AddRange(categories);
        // SaveChanges();
    }
    
    private void Seed()
    {
        // Randomizer.Seed = new Random(1039);
        // var faker = new Faker("de");
        //
        // var authors = new Faker<Author>("de").CustomInstantiator(f =>
        //     {
        //         var lastname = f.Name.LastName();
        //         return new Author(
        //                 firstname: f.Name.FirstName(),
        //                 lastname: lastname,
        //                 email: $"{lastname.ToLower()}@spengergasse.at",
        //                 username: lastname.ToLower(),
        //                 initialPassword: "1111",
        //                 phone: $"{+43}{f.Random.Int(1, 9)}{f.Random.String2(9, "0123456789")}".OrNull(f, 0.25f))
        //             { Guid = f.Random.Guid() };
        //     })
        //     .Generate(10)
        //     .GroupBy(a => a.Email).Select(g => g.First())
        //     .ToList();
        // Authors.AddRange(authors);
        // SaveChanges();
        //
        // // Use OrderBy with PK to read in a deterministic sort order!
        // var categories = Categories.OrderBy(c => c.Id).ToList();
        //
        // var articles = new Faker<Article>("de").CustomInstantiator(f =>
        //     {
        //         return new Article(
        //                 headline: f.Lorem.Sentence(f.Random.Int(2, 4)),
        //                 content: f.Lorem.Paragraphs(10, 20),
        //                 created: f.Date.Between(new DateTime(2021, 1, 1), new DateTime(2022, 1, 1)),
        //                 imageUrl: f.Image.PicsumUrl(),
        //                 author: f.Random.ListItem(authors),
        //                 category: f.Random.ListItem(categories))
        //             { Guid = f.Random.Guid() };
        //     })
        //     .Generate(6)
        //     .ToList();
        // Articles.AddRange(articles);
        // SaveChanges();
    }
}