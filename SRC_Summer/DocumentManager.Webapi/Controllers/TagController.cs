using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManager.Dto;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DocumentManager.Webapi.Controllers;

[Route("/api/tags")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly DocumentManagerContext _db;
    private readonly IMapper _mapper;
    private readonly ILogger<TagController> _logger;

    public TagController(DocumentManagerContext db, IMapper mapper, ILogger<TagController> logger)
    {
        _db = db;
        _mapper = mapper;
        _logger = logger;
    }

    // -------------------------------------------------------
    // HTTP GET
    // -------------------------------------------------------

    // Reacts to GET /api/tags
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tag))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllTags()
    {
        var tags = await _db.Tag
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                category = x.Category
            })
            .ToListAsync();
        _logger.LogInformation("Tags retrieved successfully: {Tags}", tags);
        return Ok(tags);
    }

    // Reacts to /api/tags/1
    [Authorize]
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tag))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTagDetail(int id)
    {
        var tag = await _db.Tag
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                category = x.Category
            })
            .FirstOrDefaultAsync(x => x.id == id);
        _logger.LogWarning("Tag with ID {Id} not found", id);
        if (tag is null) return NotFound();
        _logger.LogInformation("Tag with ID {Id} retrieved successfully: {Tag}", id, tag);
        return Ok(tag);
    }

    // -------------------------------------------------------
    // HTTP POST
    // -------------------------------------------------------

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tag))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTag(TagDto tagDto)
    {
        var tag = _mapper.Map<Tag>(tagDto);
        if (tag is null) return NotFound();
        _db.Tag.Add(tag);
        try
        {
            await _db.SaveChangesAsync();
            _logger.LogInformation("Tag added successfully: {Tag}", tag);
        }
        catch (DbUpdateException)
        {
            _logger.LogError("Error adding tag: {Tag}", tag);
            return BadRequest();
        }
        return Ok(_mapper.Map<Tag, TagDto>(tag));
    }

    // -------------------------------------------------------
    // HTTP PUT
    // -------------------------------------------------------

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditTag(int id, TagDto tagDto)
    {
        var tag = await _db.Tag.FirstOrDefaultAsync(a => a.Id == id);
        if (tag is null) return NotFound();
        _mapper.Map(tagDto, tag);
        try
        {
            await _db.SaveChangesAsync();
            _logger.LogInformation("EditTag method called with ID {Id} and tag {Tag}", id, tagDto);
        }
        catch (DbUpdateException)
        {
            _logger.LogError("Error updating tag with ID {Id}: {Tag}", id, tag);
            return BadRequest();
        }
        _logger.LogInformation("Tag with ID {Id} updated successfully: {Tag}", id, tag);
        return NoContent();
    }


    // -------------------------------------------------------
    // HTTP DELETE
    // -------------------------------------------------------

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteTag(int id)
    {
        var tag = await _db.Tag.FirstOrDefaultAsync(a => a.Id == id);
        if (tag is null) return NotFound();
        _db.Tag.Remove(tag);
        try
        {
            await _db.SaveChangesAsync();
            _logger.LogInformation("Tag with ID {Id} deleted successfully", id);
        }
        catch (DbUpdateException)
        {
            _logger.LogError("Error deleting tag with ID {Id}", id);
            return BadRequest();
        }

        return NoContent();
    }
}