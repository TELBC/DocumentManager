using System.Diagnostics;
using Bogus;
using Bogus.DataSets;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Priority;
using Assert = Xunit.Assert;

namespace DocumentManager.Test;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class DBTest : DocumentManagerDB
{
    [Fact, Priority(0)]
    public void CreateDatabaseTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
    }
    [Fact, Priority(1)]
    public void SeedDataTest()
    {
        _db.Database.EnsureCreated();
        // user
        var user = new User("Admin", "admin@example.com", "verysecure");
        _db.Add(user);
        //tag
        var tags = new Faker<Tag>().CustomInstantiator(faker =>
            new Tag(name: faker.System.CommonFileName(), faker.PickRandom<Category>())
        ).Generate(10);
        _db.AddRange(tags);
        //document
        var documents = new Faker<Document>().CustomInstantiator(faker => 
            new Document(title:faker.System.FileName(),
                content:faker.Lorem.Text(),
                tags:tags.OrderBy(x => new Random().Next()).Take(3).ToList(),
                type:faker.System.FileType())
        ).Generate(20);
        _db.AddRange(documents);
        //folder
        var folders = new Faker<Folder>().CustomInstantiator(faker => 
            new Folder(name:faker.System.FileName(), documents:documents.OrderBy(x => new Random().Next()).Take(5).ToList())
        ).Generate(2);
        _db.AddRange(folders);
        //friends
        var users = new Faker<User>().CustomInstantiator(faker => 
                new User(
                    name: faker.Internet.UserName(),
                    email:faker.Internet.Email(),
                    password:faker.Internet.Password()))
            .Generate(10);
        _db.AddRange(users);
        //documentmanager
        var documentManager = new Model.DocumentManager(user);
        foreach (var u in users)
        {
            documentManager.AddFriend(u);
        }
        foreach (var f in folders)
        {
            documentManager.AddFolder(f);
        }
        _db.Add(documentManager);
        _db.SaveChanges();
        Assert.True(_db.Tag.Count()==10);
        Assert.True(_db.Document.Count()==20);
        Assert.True(_db.Folder.Count()==2);
        Assert.True(_db.User.Count()==11);
        Assert.True(_db.DocumentManager.Count()==1);
    }
}