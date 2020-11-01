using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation
{
	/// <summary>
	/// Attribution: https://www.nuget.org/packages/Microsoft.AspNetCore.Components.DataAnnotations.Validation
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class ValidateComplexTypeAttribute : ValidationAttribute
	{
		readonly IValidatorKey<ObjectGraphValidator> _validator;

		public ValidateComplexTypeAttribute() : this(ValidatorKey.Default) {}

		public ValidateComplexTypeAttribute(IValidatorKey<ObjectGraphValidator> validator) => _validator = validator;

		/// <inheritdoc />
		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			_validator.Get(context)?.Validate(value, context);

			return ValidationResult.Success;
		}
	}
}
