using DragonSpark.Compose;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class MaxLengthValidator : MetadataValueValidator
{
	public MaxLengthValidator(uint length) : this(new MaxLengthAttribute(length.Degrade())) {}

	public MaxLengthValidator(ValidationAttribute metadata) : base(metadata) {}
}