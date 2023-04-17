using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManager.Dto;

public record TagDto(
    [StringLength(255, MinimumLength = 2, ErrorMessage = "The length of the name is invalid")]
    string Name,
    Category Category
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var db = validationContext.GetRequiredService<DocumentManagerContext>();
        if (db.Tag.Any(a => a.Name == Name))
        {
            yield return new ValidationResult("Tag already exists", new[] { nameof(Name) });
        }
    }
}