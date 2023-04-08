using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Xunit;
using Assert = Xunit.Assert;

namespace DocumentManager.Test;

[Collection("Sequential")]
public class DbTest : DocumentManagerDb
{
    [Fact]
    public void CreateDatabaseTest()
    {
        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
    }

    [Fact]
    public void ContextTest()//test to debug Tags in document
    {
        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
        Db.Initialize();
        Db.Seed();
    }
    

    [Fact]
    public void SeedDataTest()
    {
        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
        // user
        var user = new User("Admin", "admin@example.com", "verySecure");
        Db.Add(user);
        
        //tag
        var tags = new Faker<Tag>().CustomInstantiator(faker =>
            new Tag(faker.Lorem.Word(), faker.PickRandom<Category>())
        ).Generate(10);
        Db.AddRange(tags);
        
        //document
        var documents = new Faker<Document>().CustomInstantiator(faker =>
            {
                var document = new Document(faker.Lorem.Word(),
                    faker.Lorem.Text(),
                    faker.System.FileExt());
                return document;
            }
        ).Generate(20);
        Db.AddRange(documents);

        //folder
        var folders = new Faker<Folder>().CustomInstantiator(faker =>
        {
            var folder = new Folder(faker.Lorem.Word())
            {
                Documents = documents.OrderBy(_ => new Random().Next()).Take(18).ToList()
            };
            return folder;
        }).Generate(2);
        Db.AddRange(folders);
        //friends
        var users = new Faker<User>().CustomInstantiator(faker =>
                new User(
                    faker.Internet.Email().Split('@')[0],
                    faker.Internet.Email(),
                    faker.Internet.Password()))
            .Generate(10);
        Db.AddRange(users);
        //documentManager
        var documentManager = new Model.DocumentManager(user);
        foreach (var u in users) documentManager.AddFriend(u);
        foreach (var f in folders) documentManager.AddFolder(f);
        Db.Add(documentManager);
        
        Db.SaveChanges();
        
        //documentTag
        var documentTags = new Faker<DocumentTag>()
            .CustomInstantiator(f => new DocumentTag
            {
                DocumentId = f.PickRandom(documents).Id,
                TagId = f.PickRandom(tags).Id
            }
            )
            .Generate(10);
        Db.AddRange(documentTags);
        
        
        Db.SaveChanges();
        Assert.True(Db.Tag.Count() == 10);
        Assert.True(Db.Document.Count() == 20);
        Assert.True(Db.Folder.Count() == 2);
        Assert.True(Db.User.Count() == 11);
        Assert.True(Db.DocumentManager.Count() == 1);
    }
}