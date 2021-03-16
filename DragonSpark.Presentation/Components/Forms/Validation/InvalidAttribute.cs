using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public sealed class InvalidAttribute : ValidationAttribute
	{
		public override bool IsValid(object? value) => false;
	}
}