using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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

    public FolderController(DocumentManagerContext db)
    {
        _db = db;
    }
    // Reacts to GET /api/folders
    [HttpGet]
    public IActionResult GetAllFolders()
    {
        var folders = _db.Folder
            .Include(x => x.Documents).ThenInclude(x => x.Tags).ThenInclude(x=>x.Tag)
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                documents = x.Documents.Select(x =>new
                {
                    id = x.Id,
                    title = x.Title,
                    content = x.Content,
                    type = x.Type,
                    #pragma warning disable CS8602
                    tags = x.Tags.Select(dt => dt.Tag.Name).ToList(),
                    #pragma warning restore CS8602
                    version = x.Version
                })
            });
        return Ok(folders);
    }
    // Reacts to /api/folder/10
    [HttpGet("{id:int}")]
    public IActionResult GetFolderDetail(int id)
    {
        var folder = _db.Folder
            .Include(x => x.Documents).ThenInclude(x => x.Tags).ThenInclude(x=>x.Tag)
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                documents = x.Documents.Select(x =>new
                {
                    id = x.Id,
                    title = x.Title,
                    content = x.Content,
                    type = x.Type,
                    #pragma warning disable CS8602
                    tags = x.Tags.Select(dt => dt.Tag.Name).ToList(),
                    #pragma warning restore CS8602
                    version = x.Version
                })
            }).FirstOrDefault(x=>x.id==id);
        if (folder is null) return NotFound();
        return Ok(folder);
    }
    
    // Reacts to /api/folder/10/1
    [HttpGet("{folderId:int}/{documentId:int}")]
    public IActionResult GetDocumentInFolderDetail(int folderId, int documentId)//fix 
    {
        var document = _db.Document
            .Include(x => x.Tags).ThenInclude(x => x.Tag)
            .Where(x => x.Id == folderId && x.Id == documentId)
            .Select(x => new
            {
                id = x.Id,
                title = x.Title,
                content = x.Content,
                type = x.Type,
                #pragma warning disable CS8602
                tags = x.Tags.Select(dt => dt.Tag.Name).ToList(),
                #pragma warning restore CS8602
                version = x.Version
            })
            .FirstOrDefault();

        if (document is null) return NotFound();
        return Ok(document);
    }
}