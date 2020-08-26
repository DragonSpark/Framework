using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public class RegularExpressionValidator : MetadataValueValidator
	{
		public RegularExpressionValidator(string expression) : this(new RegularExpressionAttribute(expression)) {}

		public RegularExpressionValidator(RegularExpressionAttribute metadata) : base(metadata) {}
	}
}