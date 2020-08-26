using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public sealed class RequiredValidator : MetadataValueValidator
	{
		public static RequiredValidator Default { get; } = new RequiredValidator();

		RequiredValidator() : base(new RequiredAttribute()) {}
	}
}