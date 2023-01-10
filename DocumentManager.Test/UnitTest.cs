using System.Collections.Generic;
using System.Linq;
using DocumentManager.Infrastructure;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Assert = Xunit.Assert;

namespace DocumentManager.Test;

public class UnitTests : DBTest
{
    [Fact]
    public void CreateDatabaseTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
        Assert.True(true);
    }
    // [Fact]
    // public void CanCreateAndDeleteDocument()
    // {
    //     var opt = new DbContextOptionsBuilder<DocumentManagerContext>()
    //         .UseNpgsql("Server=localhost;Port=5432;Database=DocumentManager;User ID=jimmy;Password=54321;")
    //         .Options;
    //     AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    //     using (var context = new DocumentManagerContext(opt))
    //     {
    //         // Create the database
    //         context.Database.EnsureCreated();
    //
    //         // Delete the database
    //         context.Database.EnsureDeleted();
    //
    //         // Create a new user
    //         var user = new User("Test User", "user@example.com", "qwerty12345");
    //         context.Users.Add(user);
    //         context.SaveChanges();
    //
    //         var folder = new Folder { Name = "Test Folder" };
    //         context.Folders.Add(folder);
    //         context.SaveChanges();
    //     
    //         folder.Id = user.Id;
    //
    //         var documentManager = new DocumentManager();
    //     
    //         // Add the user to the _users field
    //         documentManager.Add(user);
    //
    //         var document = documentManager.CreateDocument(
    //             "Test Document", "Test Content", new List<Tag>(), "Test Type", folder);
    //
    //         // Add the document to the Documents table in the database
    //         context.Documents.Add(document);
    //         context.SaveChanges();
    //
    //         Assert.Equal(1, context.Documents.Count());
    //
    //         documentManager.DeleteDoc(document);
    //
    //         // The document should be deleted from the database
    //         Assert.Equal(0, context.Documents.Count());
    //     }
    // }
    //
    // [Fact]
    // public void CanSearchForDocuments()
    // {
    //     var opt = new DbContextOptionsBuilder<DocumentManagerContext>()
    //         .UseNpgsql(
    //             "Server=localhost;Port=5432;Database=DocumentManager;User ID=jimmy;Password=54321;")
    //         .Options;
    //     AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    //     using (var context = new DocumentManagerContext(opt))
    //     {
    //         context.Database.EnsureDeleted();
    //         context.Database.EnsureCreated();
    //
    //         var folder = new Folder { Name = "Test Folder" };
    //         context.Folders.Add(folder);
    //         context.SaveChanges();
    //
    //         var documentManager = new DocumentManager();
    //         var document1 = documentManager.CreateDocument(
    //             "Test Document 1", "Test Content", new List<Tag>(), "Test Type", folder);
    //         var document2 = documentManager.CreateDocument(
    //             "Test Document 2", "Test Content", new List<Tag>(), "Test Type", folder);
    //         var document3 = documentManager.CreateDocument(
    //             "Other Document", "Test Content", new List<Tag>(), "Test Type", folder);
    //
    //         // Add the documents to the Documents table in the database
    //         context.Documents.Add(document1);
    //         context.Documents.Add(document2);
    //         context.Documents.Add(document3);
    //         context.SaveChanges();
    //
    //         var searchResults = documentManager.SearchDocs("Test");
    //
    //         Assert.Equal(2, searchResults.Count);
    //         Assert.Equal("Test Document 1", searchResults[0].Title);
    //         Assert.Equal("Test Document 2", searchResults[1].Title);
    //     }
    // }
}