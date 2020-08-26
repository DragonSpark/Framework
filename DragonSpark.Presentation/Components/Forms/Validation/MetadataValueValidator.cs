using DragonSpark.Model.Selection.Conditions;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public class MetadataValueValidator : Condition<object>, IValidateValue<object>
	{
		public MetadataValueValidator(ValidationAttribute metadata) : base(metadata.IsValid) {}
	}
}