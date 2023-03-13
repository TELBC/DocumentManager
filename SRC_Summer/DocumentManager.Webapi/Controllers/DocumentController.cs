using System.Linq;
using DocumentManager.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManager.Webapi.Controllers;

[ApiController]
[Route("/api/documents")]
public class DocumentController : ControllerBase
{
    private readonly DocumentManagerContext _db;

    public DocumentController(DocumentManagerContext db)
    {
        _db = db;
    }
    // Reacts tpoGET /api/document
    [HttpGet]
    public IActionResult GetAllDocuments()
    {
        var documents = _db.Document.ToList();
        return Ok(documents);
    }
    // Reacts to /api/document/10
    [HttpGet("{id:int}")]
    public IActionResult GetDocumentDetail(int id)
    {
        var document = _db.Document.Find(id);

        if (document is null) return NotFound();
        return Ok(document);
    }
}