using Bogus;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace DocumentManager.Test;

public class RepositoryTest : DocumentManagerDB
{
    [Fact]
    public void InsertTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
        var db = new DocumentManagerDB();
        var user = new Faker<User>().CustomInstantiator(faker => 
                new User(
                    name: faker.Internet.UserName(),
                    email:faker.Internet.Email(),
                    password:faker.Internet.Password()))
            .Generate();
        var documentManager = new DocumentManager.Model.DocumentManager(user);
        DocumentManagerRepository repository = db.documentManagerRepository;
        repository.InsertOne(documentManager);
        _db.ChangeTracker.Clear();
        Assert.True(repository.Queryable.Any(d => d.Id ==documentManager.Id));
    }

    [Fact]
    public void UpdateTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
        var db = new DocumentManagerDB();
        var user = new Faker<User>().CustomInstantiator(faker => 
                new User(
                    name: faker.Internet.UserName(),
                    email:faker.Internet.Email(),
                    password:faker.Internet.Password()))
            .Generate();
        var documentManager = new DocumentManager.Model.DocumentManager(user);
        var folder = new Faker<Folder>().CustomInstantiator(faker => 
            new Folder(name:faker.System.FileName(), documents:new List<Document>{new Document(title:faker.System.FileName(),
                content:faker.Lorem.Text(),
                tags:new List<Tag>{new Tag(name: faker.System.CommonFileName(), faker.PickRandom<Category>())},
                type:faker.System.FileType())})).Generate();
        DocumentManagerRepository repository = db.documentManagerRepository;
        repository.InsertOne(documentManager);
        repository.AddFolder(1,folder);//updateOne in DocumentManagerRepository
        repository.UpdateOne(documentManager);
        _db.ChangeTracker.Clear();
        Assert.True(repository.Queryable.FirstOrDefault(d => d.Id == folder.Id)!.Folders.Count == 1);
    }

    [Fact]
    public void DeleteTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
        var db = new DocumentManagerDB();
        var user = new Faker<User>().CustomInstantiator(faker => 
                new User(
                    name: faker.Internet.UserName(),
                    email:faker.Internet.Email(),
                    password:faker.Internet.Password()))
            .Generate();
        var documentManager = new DocumentManager.Model.DocumentManager(user);
        DocumentManagerRepository repository = db.documentManagerRepository;
        repository.InsertOne(documentManager);
        repository.DeleteOne(documentManager.Id);
        _db.ChangeTracker.Clear();
        Assert.False(repository.Queryable.Any(d => d.Id == documentManager.Id));

    }
}