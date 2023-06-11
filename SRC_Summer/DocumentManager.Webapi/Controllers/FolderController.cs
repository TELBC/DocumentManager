using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
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
    public async Task<IActionResult> GetAllFolders()
    {
        var folders = await _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x => x.Tag)
            .Select(x => new
            {
                guid = x.Guid,
                name = x.Name,
                documents = x.Documents!.Select(d => new
                {
                    guid = d.Guid,
                    title = d.Title,
                    content = d.Content,
                    type = d.Type,
                    tags = d.Tags.Select(dt => dt.Tag!.Name).ToList(),
                    version = d.Version
                })
            }).ToListAsync();
        return Ok(folders);
    }

    // Reacts to /api/folder/10
    [HttpGet("{guid:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Folder))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFolderDetail(Guid guid)
    {
        var folder = await _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x => x.Tag)
            .Select(x => new
            {
                guid = x.Guid,
                name = x.Name,
                documents = x.Documents!.Select(x => new
                {
                    guid = x.Guid,
                    title = x.Title,
                    content = x.Content,
                    type = x.Type,
                    tags = x.Tags.Select(dt => dt.Tag!.Name).ToList(),
                    version = x.Version
                })
            })
            .FirstOrDefaultAsync(x => x.guid == guid);
        if (folder is null) return NotFound();
        return Ok(folder);
    }

    // Reacts to /api/folder/10/1
    [HttpGet("{folderGuid:Guid}/{docGuid:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocumentInFolderDetailAsync(Guid folderGuid, Guid docGuid)
    {
        var folder = await _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x => x.Tag)
            .FirstOrDefaultAsync(x => x.Guid == folderGuid);
        if (folder == null) return NotFound();
        var document = folder?.Documents?.Select(x => new
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
    [HttpGet("{folderGuid:Guid}/{docGuid:Guid}/tags")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tag))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocumentInFolderTags(Guid folderGuid, Guid docGuid)
    {
        var folder = await _db.Folder
            .Include(x => x.Documents)!.ThenInclude(x => x.Tags).ThenInclude(x => x.Tag)
            .FirstOrDefaultAsync(x => x.Guid == folderGuid);
        if (folder == null) return NotFound();
        var document = folder?.Documents?.FirstOrDefault(x => x.Guid == docGuid);

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
    public async Task<IActionResult> AddFolder(FolderDto folderDto)
    {
        var folder = _mapper.Map<Folder>(folderDto);
        var existingDocuments = await _db.Document.Where(d => folderDto.DocumentTitles.Contains(d.Title)).ToListAsync();
        foreach (var document in existingDocuments)
        {
            document.FolderId = folder.Id;
        }
        folder.Documents!.AddRange(existingDocuments);
        folder.DocumentManagerId = 1;
        _db.Folder.Add(folder);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest();
        }

        return Ok(folder);
    }

    // -------------------------------------------------------
    // HTTP PUT
    // -------------------------------------------------------
    [HttpPut("{guid:Guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditFolder(Guid guid, FolderDto folderDto)
    {
        if (guid != folderDto.Guid) return BadRequest();
        var folder = await _db.Folder.Include(f => f.Documents).FirstOrDefaultAsync(a => a.Guid == guid);
        if (folder is null) return NotFound();
        var existingTitles = folder.Documents!.Select(d => d.Title).ToList();
        if (existingTitles != folderDto.DocumentTitles)
        {
            //if the two lists are not the same, the folder list should be updated with the new folderDto list so that the documents in the folderdto are the ones in the folder
            var newDocumentTitles = folderDto.DocumentTitles.Except(existingTitles);
            var deletedDocuments = folder.Documents!.Where(d => !folderDto.DocumentTitles.Contains(d.Title)).ToList();

            foreach (var deletedDoc in deletedDocuments)
            {
                deletedDoc.FolderId = null;
            }
            foreach (var title in newDocumentTitles)
            {
                var newDoc = await _db.Document.Where(d => d.Title == title).FirstOrDefaultAsync();
                if (newDoc == null)
                {
                    return BadRequest();
                }
                if (!folder.Documents!.Contains(newDoc))
                {
                    folder.Documents!.Add(newDoc);
                }
            }
        }
        _mapper.Map(folderDto, folder);
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
    [HttpDelete("{guid:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteFolder(Guid guid)
    {
        var folder = await _db.Folder.FirstOrDefaultAsync(a => a.Guid == guid);
        if (folder is null) return NotFound();
        var documents = await _db.Document.Where(d => d.FolderId==folder.Id).ToListAsync();
        if (documents is null) return NotFound();
        foreach (var doc in documents)
        {
            doc.FolderId = null;
        }
        _db.Folder.Remove(folder);
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