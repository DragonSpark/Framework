using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation.Expressions
{
	public class RangeValidator : MetadataValueValidator
	{
		public RangeValidator(double minimum, double maximum) : this(new RangeAttribute(minimum, maximum)) {}

		public RangeValidator(RangeAttribute metadata) : base(metadata) {}
	}

}