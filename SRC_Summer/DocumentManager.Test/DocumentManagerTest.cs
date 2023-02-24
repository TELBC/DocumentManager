using System.Collections.Generic;
using System.Linq;
using Bogus;
using DocumentManager.Model;
using Xunit;

namespace DocumentManager.Test;

[Collection("Sequential")]
public class DocumentManagerTest : DocumentManagerDb
{
    [Fact]
    public void AddFolderTest()
    {
        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
        var user = new Faker<User>().CustomInstantiator(faker =>
                new User(
                    faker.Internet.Email().Split('@')[0],
                    faker.Internet.Email(),
                    faker.Internet.Password()))
            .Generate();
        var documentManager = new Model.DocumentManager(user);
        var folder = new Faker<Folder>().CustomInstantiator(faker =>
        {
            var folder = new Folder(faker.System.FileName())
            {
                Documents = new List<Document>()
            };
            var document = new Document(faker.Lorem.Word(),
                faker.Lorem.Text(),
                faker.System.FileType())
            {
                Tags = new List<Tag> { new(faker.Lorem.Word(), faker.PickRandom<Category>()) }
            };
            folder.Documents.Add(document);
            return folder;
        }).Generate();
        documentManager.AddFolder(folder);
        Db.Add(documentManager);
        Db.SaveChanges();
        Assert.True(Db.Folder.Count() == 1);
    }

    [Fact]
    public void AddFriendsTest()
    {
        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
        var user = new Faker<User>().CustomInstantiator(faker =>
                new User(
                    faker.Internet.Email().Split('@')[0],
                    faker.Internet.Email(),
                    faker.Internet.Password()))
            .Generate();
        var documentManager = new Model.DocumentManager(user);
        var users = new Faker<User>().CustomInstantiator(faker =>
                new User(
                    faker.Internet.Email().Split('@')[0],
                    faker.Internet.Email(),
                    faker.Internet.Password()))
            .Generate(5);
        foreach (var u in users) documentManager.AddFriend(u);
        Db.Add(documentManager);
        Db.SaveChanges();
        Assert.True(Db.UserBase.Count() == 6);
    }
}