using System;
using System.Linq;
using AutoMapper;
using DocumentManager.Dto;
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
    private readonly IMapper _mapper;

    public DocumentController(DocumentManagerContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    // -------------------------------------------------------
    // HTTP GET
    // -------------------------------------------------------

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
    
    // -------------------------------------------------------
    // HTTP POST
    // -------------------------------------------------------
    
    [HttpPost]
    public IActionResult AddDocument(DocumentDto documentDto)
    {
        var document = _mapper.Map<Document>(documentDto);
        _db.Document.Add(document);
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return Ok(_mapper.Map<Document, DocumentDto>(document));
    }
    
    // -------------------------------------------------------
    // HTTP PUT
    // -------------------------------------------------------
    
    [HttpPut("{title}")]
    public IActionResult EditDocument(string title, DocumentDto documentDto)
    {
        // if (title != documentDto.Title) { return BadRequest(); }
        var document = _db.Document.FirstOrDefault(a => a.Title == title);
        _mapper.Map(documentDto, document);
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }
    
    
    // -------------------------------------------------------
    // HTTP DELETE
    // -------------------------------------------------------
    
    [HttpDelete("{id:int}")]
    public IActionResult DeleteDocument(int id)
    {
        var document = _db.Document.FirstOrDefault(a => a.Id == id);
        if (document is null) { return NotFound(); }
        // TODO: Remove referenced data (if needed)
        _db.Document.Remove(document);
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }
}