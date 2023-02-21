using DocumentManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Test;

public class DocumentManagerDb
{
    protected readonly DocumentManagerContext Db;
    public readonly DocumentManagerRepository DocumentManagerRepository;

    public DocumentManagerDb()
    {
        var opt = new DbContextOptionsBuilder<DocumentManagerContext>()
            .UseNpgsql(
                "Host=localhost;Port=5432;Database=DocumentManager;Username=postgres;Password=pwd;IncludeErrorDetail=true;")
            .Options;

        Db = new DocumentManagerContext(opt);
        DocumentManagerRepository = new DocumentManagerRepository(Db);
    }

    public void Dispose()
    {
        Db.Dispose();
    }
}