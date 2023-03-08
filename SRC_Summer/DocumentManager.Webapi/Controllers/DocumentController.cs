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
    // Reagiert auf GET /api/document
    [HttpGet]
    public IActionResult GetAllDocuments()
    {
        return Ok(new string[]{"Document 10", "Document 11"});
    }
    // Reagiert z. B. auf /api/document/10
    [HttpGet("{id:int}")]
    public IActionResult GetDocumentDetail(int id)
    {
        // if (id < 1000) { return NotFound(); }
        return Ok($"Document {id}");
    }
}