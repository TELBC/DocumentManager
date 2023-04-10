using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManager.Dto;

public record DocumentDto(
    [StringLength(255, MinimumLength = 10, ErrorMessage = "The length of the title is invalid")]
    string Title,
    List<DocumentTag> Tags,
    string Type,
    [StringLength(65535, MinimumLength = 0, ErrorMessage = "The content length exceeds the limit")]
    string Content
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var db = validationContext.GetRequiredService<DocumentManagerContext>();
        if (db.Document.Any(a => a.Title == Title))
        {
            yield return new ValidationResult("Document already exists", new[] { nameof(Title) });
        }
    }
}
