using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Infrastructure;

public class DocumentManagerDB
{
    protected readonly DocumentManagerContext _db;
    public DocumentManagerRepository documentManagerRepository;

    public DocumentManagerDB()
    {
        var opt = new DbContextOptionsBuilder<DocumentManagerContext>()
            .UseNpgsql(
                "Host=localhost;Port=5432;Database=DocumentManager;Username=postgres;Password=pwd;IncludeErrorDetail=true;")
            .Options;

        _db = new DocumentManagerContext(opt);
        documentManagerRepository = new DocumentManagerRepository(_db);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}