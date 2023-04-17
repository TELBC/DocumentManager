using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using AutoMapper;
using DocumentManager.Dto;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAllDocuments()
    {
        var documents = _db.Document
            .Include(x => x.Tags).ThenInclude(x => x.Tag)
            .Select(x => new
            {
                guid = x.Guid,
                title = x.Title,
                tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
                type = x.Type,
                content = x.Content,
                version = x.Version,
            });
        return Ok(documents);
    }

    // Reacts to /api/documents/10
    [HttpGet("{guid:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetDocumentDetail(Guid guid)
    {
        var document = _db.Document
            .Include(x => x.Tags).ThenInclude(x => x.Tag)
            .Select(x => new
            {
                guid = x.Guid,
                title = x.Title,
                content = x.Content,
                type = x.Type,
                tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
                version = x.Version
            })
            .FirstOrDefault(x => x.guid == guid);
        if (document is null) return NotFound();
        return Ok(document);
    }

    //Reacts to /api/documents/1/tags/
    [HttpGet("{guid:Guid}/tags")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tag))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetDocumentTags(Guid guid)
    {
        var tags = _db.Document
            .Include(x => x.Tags).ThenInclude(x => x.Tag)
            .FirstOrDefault(x => x.Guid == guid)?.Tags
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
    // [Authorize]
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    
    // [Authorize]
    [HttpPut("{guid:Guid}")]//fix
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult EditDocument(Guid guid, DocumentDto documentDto)//documentTag problems
    {
        if (guid != documentDto.Guid) { return BadRequest(); }
        var document = _db.Document.FirstOrDefault(a => a.Guid == guid);
        if (document is null) { return NotFound();}

        var tags = _db.DocumentTag.Where(dt => dt.Document!.Guid == guid).ToList();
        foreach (var tag in tags)
        {
            _db.DocumentTag.Remove(tag);
        }
        _mapper.Map(documentDto, document);
        foreach (var tag in tags)
        {
            _db.DocumentTag.Add(tag);
        }
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }
    
    
    // -------------------------------------------------------
    // HTTP DELETE
    // -------------------------------------------------------
    
    // [Authorize]
    [HttpDelete("{guid:Guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteDocument(Guid guid)
    {
        var document = _db.Document.FirstOrDefault(a => a.Guid == guid);
        if (document is null) { return NotFound(); }
        var tags = _db.DocumentTag.Where(dt => dt.Document!.Guid == guid).ToList();
        foreach (var tag in tags)
        {
            _db.DocumentTag.Remove(tag);   
        }
        _db.Document.Remove(document);
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }
    
}