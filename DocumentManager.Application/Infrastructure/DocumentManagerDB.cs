using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Infrastructure;

public class DocumentManagerDB
{
    private readonly DocumentManagerContext _db;

    public DocumentManagerDB()
    {
             var opt = new DbContextOptionsBuilder<DocumentManagerContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=pwd;IncludeErrorDetail=true;")
            .Options;

        _db = new DocumentManagerContext(opt);
       
    }


//     public void Seed()
//     {
//         // Create the database
//         _db.Database.EnsureCreated();
//
//         // Delete the database
//         _db.Database.EnsureDeleted();
//
//         // Create a new user
//         var user = new User("Test User", "user@example.com", "qwerty12345");
//         _db.Users.Add(user);
//         _db.SaveChanges();
//
//         var folder = new Folder { Name = "Test Folder" };
//         _db.Folders.Add(folder);
//         _db.SaveChanges();
//         
//         folder.Id = user.Id;
//
//         var documentManager = new Model.DocumentManager();
//         
//         // Add the user to the _users field
//         documentManager.Add(user);
//
//         var document = documentManager.CreateDocument(
//             "Test Document", "Test Content", new List<Tag>(), "Test Type", folder);
//
//         // Add the document to the Documents table in the database
//         _db.Documents.Add(document);
//         _db.SaveChanges();
//     }
}