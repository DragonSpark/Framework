using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components
{
	/// <summary>
	/// Attribution: https://www.nuget.org/packages/Microsoft.AspNetCore.Components.DataAnnotations.Validation
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ValidateComplexTypeAttribute : ValidationAttribute
	{
		/// <inheritdoc />
		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			ObjectGraphDataAnnotationsValidator.TryValidateRecursive(value, context);

			// Validation of the properties on the complex type are responsible for adding their own messages.
			// Therefore, we can always return success from here.
			return ValidationResult.Success;
		}
	}
}
