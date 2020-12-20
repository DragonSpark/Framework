using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation.Expressions
{
	public sealed class RequiredValidator : MetadataValueValidator
	{
		public static RequiredValidator Default { get; } = new RequiredValidator();

		RequiredValidator() : base(new RequiredAttribute()) {}
	}
}