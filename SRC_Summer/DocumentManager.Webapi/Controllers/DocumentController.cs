using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Bogus.DataSets;
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
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllDocuments()
    {
        var documents = await _db.Document
            .Include(x => x.Tags).ThenInclude(x => x.Tag)
            .Select(x => new
            {
                guid = x.Guid,
                title = x.Title,
                tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
                type = x.Type,
                content = x.Content,
                version = x.Version
            }).ToListAsync();
        return Ok(documents);
    }

    // Reacts to /api/documents/10
    [HttpGet("{guid:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocumentDetail(Guid guid)
    {
        var document = await _db.Document
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
            .FirstOrDefaultAsync(x => x.guid == guid);
        if (document is null) return NotFound();
        return Ok(document);
    }

    //Reacts to /api/documents/1/tags/
    [HttpGet("{guid:Guid}/tags")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tag))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocumentTags(Guid guid)
    {
        var document = await _db.Document
            .Include(x => x.Tags).ThenInclude(x => x.Tag)
            .FirstOrDefaultAsync(x => x.Guid == guid);
        var tags = document?.Tags
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
    public async Task<IActionResult> AddDocument(DocumentDto documentDto)
    {
        var document = _mapper.Map<Document>(documentDto);
        _db.Document.Add(document);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest();
        }

        return Ok(document);
    }

    // -------------------------------------------------------
    // HTTP PUT
    // -------------------------------------------------------
    // [Authorize]
    [HttpPut("{guid:Guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditDocument(Guid guid, DocumentDto documentDto)
    {
        if (guid != documentDto.Guid) return BadRequest();
        var document = await _db.Document.Include(d => d.Tags).FirstOrDefaultAsync(a => a.Guid == guid);
        if (document is null) return NotFound();

        _mapper.Map(documentDto, document);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest();
        }

        return NoContent();
    }


    // -------------------------------------------------------
    // HTTP DELETE
    // -------------------------------------------------------

    // [Authorize]
    [HttpDelete("{guid:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDocument(Guid guid)
    {
        var document = await _db.Document.Include(d => d.Tags).FirstOrDefaultAsync(a => a.Guid == guid);
        if (document is null) return NotFound();
        var tags = await _db.DocumentTag.Where(dt => dt.Document!.Guid == guid).ToListAsync();
        foreach (var tag in tags) _db.DocumentTag.Remove(tag);
        _db.Document.Remove(document);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest();
        }

        return NoContent();
    }
}