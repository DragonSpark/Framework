using System.ComponentModel.DataAnnotations;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Components.Validation.Expressions;

public class MetadataValueValidator : MetadataValueValidator<object>
{
    public MetadataValueValidator(ValidationAttribute metadata) : base(metadata) {}
}

public class MetadataValueValidator<T> : Condition<T?>, IValidateValue<T>
{
	public MetadataValueValidator(ValidationAttribute metadata) : base(x => metadata.IsValid(x)) {}
}