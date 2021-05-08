using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation.Expressions
{
	public class RangeValidator : MetadataValueValidator, IValidateValue<decimal>
	{
		public RangeValidator(double minimum, double maximum) : this(new RangeAttribute(minimum, maximum)) {}

		public RangeValidator(RangeAttribute metadata) : base(metadata) {}

		public bool Get(decimal parameter) => base.Get(parameter);
	}

}