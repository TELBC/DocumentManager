using System;
using System.Collections.Generic;
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
    public DbSet<DocumentTag> DocumentTag => Set<DocumentTag>();

    public DbSet<Model.DocumentManager> DocumentManager => Set<Model.DocumentManager>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // value converter
        modelBuilder
            .Entity<Tag>()
            .Property(c => c.Category)
            .HasConversion<String>();
        modelBuilder.Entity<User>().ToTable("User");
        
        modelBuilder.Entity<DocumentTag>()
            .HasKey(ba => new { ba.DocumentId, ba.TagId });
        modelBuilder.Entity<DocumentTag>()
            .HasOne(ba => ba.Document)
            .WithMany(b => b.Tags)
            .HasForeignKey(ba => ba.DocumentId);
        modelBuilder.Entity<DocumentTag>()
            .HasOne(ba => ba.Tag)
            .WithMany(a => a.Documents)
            .HasForeignKey(ba => ba.TagId);
        
        
        // additional config
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var key in entityType.GetForeignKeys())
                key.DeleteBehavior = DeleteBehavior.Restrict;
            foreach (var prop in entityType.GetDeclaredProperties())
            {
                if (prop.Name == "Guid")
                {
                    modelBuilder.Entity(entityType.ClrType).HasAlternateKey("AdminKey");
                    prop.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                }
                if (prop.ClrType == typeof(string) && prop.GetMaxLength() is null) prop.SetMaxLength(null);//by giving it a 255 char limit i get an error, i will troubleshoot this later
                if (prop.ClrType == typeof(DateTime)) prop.SetPrecision(3);
                if (prop.ClrType == typeof(DateTime?)) prop.SetPrecision(3);
            }
        }
    }
    
    public void CreateDatabase(bool isDevelopment)
    {
        if (isDevelopment) { Database.EnsureDeleted(); }
        if (Database.EnsureCreated()) { Initialize(); }
        if (isDevelopment) Seed();
    }
    
    public void Initialize()
    {
        var user = new User("Admin", "admin@example.com", "verySecure");
        User.Add(user);
        var documentManager = new Model.DocumentManager(user);
        DocumentManager.Add(documentManager);
        SaveChanges();
    }
    
    public void Seed()
    {
        //tag
        var tags = new Faker<Tag>().CustomInstantiator(faker =>
            new Tag(faker.Lorem.Word(), faker.PickRandom<Category>())
        ).Generate(10);
        Tag.AddRange(tags);
        
        //document
        var documents = new Faker<Document>().CustomInstantiator(faker =>
            {
                var document = new Document("Document " + faker.UniqueIndex,
                    faker.Lorem.Text(),
                    faker.System.FileExt());
                return document;
            }
        ).Generate(20);
        Document.AddRange(documents);
        
        SaveChanges();
        
        //documentTag
        var documentTags = new Faker<DocumentTag>()
            .CustomInstantiator(f => new DocumentTag {
                DocumentId = f.PickRandom(documents).Id,
                TagId = f.PickRandom(tags).Id
            })
            .Generate(19);
        DocumentTag.AddRange(documentTags);
        SaveChanges();
        
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