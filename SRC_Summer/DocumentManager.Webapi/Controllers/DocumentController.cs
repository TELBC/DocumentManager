using System;
using System.Linq;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    // Reacts to GET /api/documents
    [HttpGet]
    public IActionResult GetAllDocuments()
    {
        var documents = _db.Document
            .Include(x => x.Tags).ThenInclude(x => x.Tag)
            .Select(x => new
            {
                id = x.Id,
                title = x.Title,
                tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
                type = x.Type,
                content = x.Content,
                version = x.Version
            });
        return Ok(documents);
    }

    // Reacts to /api/documents/10
    [HttpGet("{id:int}")]
    public IActionResult GetDocumentDetail(int id)
    {
        var document = _db.Document
            .Include(x => x.Tags).ThenInclude(x => x.Tag)
            .Select(x => new
            {
                id = x.Id,
                title = x.Title,
                content = x.Content,
                type = x.Type,
                tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
                version = x.Version
            })
            .FirstOrDefault(x => x.id == id);
        if (document is null) return NotFound();
        return Ok(document);
    }

    //Reacts to /api/documents/1/tags/
    [HttpGet("{id:int}/tags")]
    public IActionResult GetDocumentTags(int id)
    {
        var tags = _db.Document
            .Include(x => x.Tags).ThenInclude(x => x.Tag)
            .FirstOrDefault(x => x.Id == id)?.Tags
            .Select(x => new
            {
                id = x.TagId,
                name = x.Tag?.Name,
                category = x.Tag?.Category
            });

        if (tags == null) return NotFound();
        return Ok(tags);
    }
}