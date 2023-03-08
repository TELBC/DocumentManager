using System;
using System.Linq;
using Bogus;
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
        var user = new User("Admin", "admin@example.com", "verySecure");
        User.Add(user);
        var documentManager = new Model.DocumentManager(user);
        DocumentManager.Add(documentManager);
        SaveChanges();
    }
    
    private void Seed()
    {
        //tag
        var tags = new Faker<Tag>().CustomInstantiator(faker =>
            new Tag(faker.Lorem.Word(), faker.PickRandom<Category>())
        ).Generate(10);
        Tag.AddRange(tags);
        //document
        var documents = new Faker<Document>().CustomInstantiator(faker =>
            {
                var document = new Document(faker.Lorem.Word(),
                    faker.Lorem.Text(),
                    faker.System.FileExt())
                {
                    Tags = tags.OrderBy(_ => new Random().Next()).Take(3).ToList()
                };
                return document;
            }
        ).Generate(20);

        Document.AddRange(documents);
        //folder
        var folders = new Faker<Folder>().CustomInstantiator(faker =>
        {
            var folder = new Folder(faker.Lorem.Word())
            {
                Documents = documents.OrderBy(_ => new Random().Next()).Take(18).ToList()
            };
            return folder;
        }).Generate(2);
        Folder.AddRange(folders);
        //users
        var users = new Faker<User>().CustomInstantiator(faker =>
                new User(
                    faker.Internet.Email().Split('@')[0],
                    faker.Internet.Email(),
                    faker.Internet.Password()))
            .Generate(10);
        User.AddRange(users);
        var docMan = DocumentManager.First();
        foreach (var u in users) docMan.AddFriend(u);
        foreach (var f in folders) docMan.AddFolder(f);
        SaveChanges();
    }
}