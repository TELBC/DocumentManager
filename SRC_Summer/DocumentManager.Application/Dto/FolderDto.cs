using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManager.Dto;

public record FolderDto(
    Guid Guid,
    [StringLength(255, MinimumLength = 10, ErrorMessage = "The length of the name is invalid")]
    string Name,
    List<Document> Documents
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var db = validationContext.GetRequiredService<DocumentManagerContext>();
        if (db.Folder.Any(a => a.Name == Name))
            yield return new ValidationResult("Folder already exists", new[] { nameof(Name) });
    }
}