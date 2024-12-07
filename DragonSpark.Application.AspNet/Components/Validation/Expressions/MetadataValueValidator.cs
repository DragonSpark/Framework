using DragonSpark.Model.Selection.Conditions;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation.Expressions;

public class MetadataValueValidator : Condition<object>, IValidateValue<object>
{
	public MetadataValueValidator(ValidationAttribute metadata) : base(metadata.IsValid) {}
}