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
        var documentmanager = new DocumentManager.Model.DocumentManager(user);
        DocumentManagerRepository repository = db.documentManagerRepository;
        repository.InsertOne(documentmanager);
        _db.ChangeTracker.Clear();
        Assert.True(repository.queryable.Where(d=>d.Id ==documentmanager.Id).Any());
    }
}