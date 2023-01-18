using Bogus;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace DocumentManager.Test;

public class DocumentManagerTest : DocumentManagerDB
{
    [Fact]
    public void AddFolderTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
        var user = new Faker<User>().CustomInstantiator(faker =>
                new User(
                    faker.Internet.UserName(),
                    faker.Internet.Email(),
                    faker.Internet.Password()))
            .Generate();
        var documentmanager = new DocumentManager.Model.DocumentManager(user);
        var folder = new Faker<Folder>().CustomInstantiator(faker =>
            new Folder(faker.System.FileName(), new List<Document>
                {
                    new(faker.System.FileName(),
                        faker.Lorem.Text(),
                        new List<Tag> { new(faker.System.CommonFileName(), faker.PickRandom<Category>()) },
                        faker.System.FileType())
                }
            )).Generate();
        documentmanager.AddFolder(folder);
        _db.Add(documentmanager);
        _db.SaveChanges();
        Assert.True(_db.Folder.Count()==1);
    }

    [Fact]
    public void AddFriendsTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
        var user = new Faker<User>().CustomInstantiator(faker =>
                new User(
                    faker.Internet.UserName(),
                    faker.Internet.Email(),
                    faker.Internet.Password()))
            .Generate();
        var documentManager = new DocumentManager.Model.DocumentManager(user);
        var users = new Faker<User>().CustomInstantiator(faker => 
                new User(
                    name: faker.Internet.UserName(),
                    email:faker.Internet.Email(),
                    password:faker.Internet.Password()))
            .Generate(5);
        foreach (var u in users)
        {
            documentManager.AddFriend(u);
        }
        _db.Add(documentManager);
        _db.SaveChanges();
        Assert.True(_db.UserBase.Count()==6);
    }
}