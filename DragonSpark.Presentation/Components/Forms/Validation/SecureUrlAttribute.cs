using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Presentation.Components.Forms.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class SecureUrlAttribute : DataTypeAttribute
{
	public SecureUrlAttribute(string message = "The {0} field is not a valid fully-qualified https URL.")
		: base(DataType.Url)
		=> ErrorMessage = message;

	public override bool IsValid(object? value)
		=> value == null || value is string path && path.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
}