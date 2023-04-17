using System.Collections.Generic;
using System.Linq;
using Bogus;
using DocumentManager.Model;

namespace DocumentManager.Test;

[Collection("Sequential")]
public class RepositoryTest : DocumentManagerDb
{
    [Fact]
    public void InsertTest()
    {
        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
        var db = new DocumentManagerDb();
        var user = new Faker<User>().CustomInstantiator(faker =>
                new User(
                    faker.Internet.Email().Split('@')[0],
                    faker.Internet.Email(),
                    faker.Internet.Password()))
            .Generate();
        var documentManager = new Model.DocumentManager(user);
        var repository = db.DocumentManagerRepository;
        repository.InsertOne(documentManager);
        Db.ChangeTracker.Clear();
        Assert.True(repository.Queryable.Any(d => d.Id == documentManager.Id));
    }

    [Fact]
    public void UpdateTest()
    {
        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
        var db = new DocumentManagerDb();
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
            var document = new Document(faker.System.FileName(), faker.Lorem.Text(), faker.System.FileType());
            folder.Documents.Add(document);
            return folder;
        }).Generate();
        var repository = db.DocumentManagerRepository;
        repository.InsertOne(documentManager);
        repository.AddFolder(1, folder); //updateOne in DocumentManagerRepository
        repository.UpdateOne(documentManager);
        Db.ChangeTracker.Clear();
        Assert.True(repository.Queryable.FirstOrDefault(d => d.Id == folder.Id)!.Folders.Count == 1);
    }

    [Fact]
    public void DeleteTest()
    {
        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
        var db = new DocumentManagerDb();
        var user = new Faker<User>().CustomInstantiator(faker =>
                new User(
                    faker.Internet.Email().Split('@')[0],
                    faker.Internet.Email(),
                    faker.Internet.Password()))
            .Generate();
        var documentManager = new Model.DocumentManager(user);
        var repository = db.DocumentManagerRepository;
        repository.InsertOne(documentManager);
        repository.DeleteOne(documentManager.Id);
        Db.ChangeTracker.Clear();
        Assert.False(repository.Queryable.Any(d => d.Id == documentManager.Id));
    }
}