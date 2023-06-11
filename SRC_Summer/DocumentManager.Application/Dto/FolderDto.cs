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
    [StringLength(80, MinimumLength = 1, ErrorMessage = "The length of the name is invalid")]
    string Name,
    List<string> DocumentTitles
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var db = validationContext.GetRequiredService<DocumentManagerContext>();
        var folder = validationContext.ObjectInstance as FolderDto;
        if (folder == null)
            yield return new ValidationResult("Invalid object type", new[] { nameof(FolderDto) });
    }
}