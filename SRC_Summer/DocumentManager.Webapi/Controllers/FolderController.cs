using System.Linq;
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
            .Include(f => f.Documents)
            .Select(f => new Folder
            {
                Id = f.Id,
                Name = f.Name,
                Documents = f.Documents
            }).ToList();
        return Ok(folders);
    }
    // Reacts to /api/folder/10
    [HttpGet("{id:int}")]
    public IActionResult GetFolderDetail(int id)
    {
        var folder = _db.Folder.Include(f => f.Documents).FirstOrDefault(f => f.Id == id);

        if (folder is null) return NotFound();
        return Ok(folder);
    }
    
    // Reacts to /api/folder/10/1
    [HttpGet("{folderId:int}/{documentId:int}")]
    public IActionResult GetDocumentInFolderDetail(int folderId, int documentId)
    {
        var folder = _db.Folder.Include(f => f.Documents).FirstOrDefault(f => f.Id == folderId);

        if (folder is null) return NotFound();
        var document = folder.Documents.FirstOrDefault(d => d.Id == documentId);
        if (document is null) return NotFound();
        return Ok(document);
    }
}