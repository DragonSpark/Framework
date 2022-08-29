using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class UriAddressValidator : MetadataValueValidator
{
	public static UriAddressValidator Default { get; } = new();

	UriAddressValidator() : base(new UrlAttribute()) {}
}