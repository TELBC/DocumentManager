using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using DocumentManager.Dto;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
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
    public IActionResult GetAllFolders()
    {
        var folders = _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x=>x.Tag)
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                documents = x.Documents!.Select(x =>new
                {
                    id = x.Id,
                    title = x.Title,
                    content = x.Content,
                    type = x.Type,
                    tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
                    version = x.Version,
                    guid = x.Guid
                })
            });
        return Ok(folders);
    }
    // Reacts to /api/folder/10
    [HttpGet("{id:int}")]
    public IActionResult GetFolderDetail(int id)
    {
        var folder = _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x=>x.Tag)
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                documents = x.Documents!.Select(x =>new
                {
                    id = x.Id,
                    title = x.Title,
                    content = x.Content,
                    type = x.Type,
                    tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
                    version = x.Version,
                    guid = x.Guid
                })
            })
    .FirstOrDefault(x=>x.id==id);
        if (folder is null) return NotFound();
        return Ok(folder);
    }
    
    // Reacts to /api/folder/10/1
    [HttpGet("{folderId:int}/{documentId:int}")]
    public IActionResult GetDocumentInFolderDetail(int folderId, int documentId)
    {
        var folder = _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x => x.Tag)
            .FirstOrDefault(x => x.Id == folderId)?.Documents;
        if (folder == null) return NotFound();
        var document = folder.Select(x => new
        {
            id = x.Id,
            title = x.Title,
            content = x.Content,
            type = x.Type,
            tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
            version = x.Version,
            guid = x.Guid
        }).FirstOrDefault(x => x.id == documentId);

        if (document == null) return NotFound();
        return Ok(document);
    }
    
    // Reacts to /api/folder/10/1/tags
    [HttpGet("{folderId:int}/{documentId:int}/tags")]
    public IActionResult GetDocumentInFolderTags(int folderId, int documentId)
    {
        var folder = _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x => x.Tag)
            .FirstOrDefault(x => x.Id == folderId)?.Documents;
        if (folder == null) return NotFound();
        var document = folder.FirstOrDefault(x => x.Id == documentId);

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
    public IActionResult EditFolder(Guid guid, FolderDto folderDto)//works but fix creation of document bug
    {
        if (guid != folderDto.Guid) { return BadRequest(); }
        var folder = _db.Folder.FirstOrDefault(a => a.Guid == guid);
        _mapper.Map(folderDto, folder);
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }
    
    // -------------------------------------------------------
    // HTTP DELETE
    // -------------------------------------------------------
    
    [HttpDelete("{id:int}")]
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