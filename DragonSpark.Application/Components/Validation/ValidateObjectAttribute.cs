using System.Linq;

namespace DragonSpark.Application.Components.Validation;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ValidateObjectAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not null)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value);

            Validator.TryValidateObject(value, context, results, validateAllProperties: true);

            if (results.Count != 0)
            {
                return CreateResult(validationContext, results);
            }
        }

        return ValidationResult.Success;
    }

    ValidationResult CreateResult(ValidationContext validationContext, List<ValidationResult> results)
    {
        var names = new List<string>();

        foreach (var result in results)
        {
            if (result.MemberNames.Any())
            {
                foreach (var name in result.MemberNames)
                {
                    names.Add($"{validationContext.MemberName}.{name}");
                }
            }
            else
            {
                names.Add(validationContext.MemberName!);
            }
        }

        return new(ErrorMessage ?? $"Validation failed for {validationContext.MemberName}", names);
    }
}