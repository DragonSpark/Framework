using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation.Expressions
{
	public class RegularExpressionValidator : MetadataValueValidator
	{
		public RegularExpressionValidator(string expression) : this(new RegularExpressionAttribute(expression)) {}

		public RegularExpressionValidator(RegularExpressionAttribute metadata) : base(metadata) {}
	}
}