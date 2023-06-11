using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManager.Dto;

public record DocumentDto(
    Guid Guid,
    [StringLength(80, MinimumLength = 1, ErrorMessage = "The length of the title is invalid")]
    string Title,
    List<DocumentTag> Tags,
    string Type,
    [StringLength(65535, ErrorMessage = "The content length exceeds the limit")]
    string Content,
    int Version
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var db = validationContext.GetRequiredService<DocumentManagerContext>();
        var document = validationContext.ObjectInstance as DocumentDto;
        
        if (document == null)
        {
            yield return new ValidationResult("Invalid object type", new[] { nameof(DocumentDto) });
        }
        else
        {
            if (db.Document.Any(a => a.Title == document.Title && a.Guid != document.Guid))
            {
                yield return new ValidationResult("Document already exists", new[] { nameof(Title) });
            }

            if (string.IsNullOrEmpty(document.Title))
            {
                yield return new ValidationResult("Title cannot be empty", new[] { nameof(Title) });
            }

            if (string.IsNullOrEmpty(document.Type))
            {
                yield return new ValidationResult("Type cannot be empty", new[] { nameof(Type) });
            }
            
            var tagIds = document.Tags.Select(t => t.TagId).ToList();
            var existingTagIds = db.Tag.Where(t => tagIds.Contains(t.Id)).Select(t => t.Id).ToHashSet();
            var invalidTags = tagIds.Where(tagId => !existingTagIds.Contains(tagId)).ToList();

            if (invalidTags.Count > 0)
            {
                yield return new ValidationResult($"Tag/s do not exist in the database: {string.Join(", ", invalidTags)}", new[] { nameof(Tags) });
            }
        }
    }
}
