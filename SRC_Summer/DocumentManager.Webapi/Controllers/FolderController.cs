using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using DocumentManager.Dto;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Webapi.Controllers;

[ApiController]
[Route("/api/folders")]
public class FolderController : ControllerBase
{
    private readonly DocumentManagerContext _db;
    private readonly IMapper _mapper;

    public FolderController(DocumentManagerContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    // -------------------------------------------------------
    // HTTP GET
    // -------------------------------------------------------

    // Reacts to GET /api/folders
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Folder))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAllFolders()
    {
        var folders = _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x=>x.Tag)
            .Select(x => new
            {
                guid = x.Guid,
                name = x.Name,
                documents = x.Documents!.Select(x =>new
                {
                    guid = x.Guid,
                    title = x.Title,
                    content = x.Content,
                    type = x.Type,
                    tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
                    version = x.Version
                })
            });
        return Ok(folders);
    }
    // Reacts to /api/folder/10
    [HttpGet("{guid:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Folder))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetFolderDetail(Guid guid)
    {
        var folder = _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x=>x.Tag)
            .Select(x => new
            {
                guid = x.Guid,
                name = x.Name,
                documents = x.Documents!.Select(x =>new
                {
                    guid = x.Guid,
                    title = x.Title,
                    content = x.Content,
                    type = x.Type,
                    tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
                    version = x.Version
                })
            })
    .FirstOrDefault(x=>x.guid==guid);
        if (folder is null) return NotFound();
        return Ok(folder);
    }
    
    // Reacts to /api/folder/10/1
    [HttpGet("{folderGuid:Guid}/{docguid:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetDocumentInFolderDetail(Guid folderGuid, Guid docGuid)
    {
        var folder = _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x => x.Tag)
            .FirstOrDefault(x => x.Guid == folderGuid)?.Documents;
        if (folder == null) return NotFound();
        var document = folder.Select(x => new
        {
            guid = x.Guid,
            title = x.Title,
            content = x.Content,
            type = x.Type,
            tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
            version = x.Version
        }).FirstOrDefault(x => x.guid == docGuid);

        if (document == null) return NotFound();
        return Ok(document);
    }
    
    // Reacts to /api/folder/10/1/tags
    [HttpGet("{folderGuid:Guid}/{docguid:Guid}/tags")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tag))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetDocumentInFolderTags(Guid folderGuid, Guid docGuid)
    {
        var folder = _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x => x.Tag)
            .FirstOrDefault(x => x.Guid == folderGuid)?.Documents;
        if (folder == null) return NotFound();
        var document = folder.FirstOrDefault(x => x.Guid == docGuid);

        var tags = document?.Tags.Select(x => new
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
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Folder))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddFolder(FolderDto folderDto)
    {
        var folder = _mapper.Map<Folder>(folderDto);
        _db.Folder.Add(folder);
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return Ok(_mapper.Map<Folder, FolderDto>(folder));
    }
    
    // -------------------------------------------------------
    // HTTP PUT
    // -------------------------------------------------------
    
    [HttpPut("{guid:Guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult EditFolder(Guid guid, FolderDto folderDto)//works but fix creation of document bug
    {
        if (guid != folderDto.Guid) { return BadRequest(); }
        var folder = _db.Folder.FirstOrDefault(a => a.Guid == guid);
        if (folder is null) { return NotFound();}
        _mapper.Map(folderDto, folder);
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }
    
    // -------------------------------------------------------
    // HTTP DELETE
    // -------------------------------------------------------
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteFolder(int id)//fix
    {
        var folder = _db.Folder.FirstOrDefault(a => a.Id == id);
        if (folder is null) { return NotFound(); }
        var documents = folder.Documents;
        if (documents is null) { return NotFound(); }
        foreach (var document in documents)
        { _db.Document.Remove(document); }
        _db.Folder.Remove(folder);
        foreach (var document in documents)
        { _db.Document.Add(document); }
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }
}