using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation;

/// <summary>
/// Attribution: https://www.nuget.org/packages/Microsoft.AspNetCore.Components.DataAnnotations.Validation
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ValidateComplexTypeAttribute : ValidationAttribute
{
	readonly IValidationContexts                 _contexts;
	readonly IValidatorKey<ObjectGraphValidator> _validator;

	public ValidateComplexTypeAttribute()
		: this(ValidationContexts.Default, ValidatorKey.Default) {}

	public ValidateComplexTypeAttribute(IValidationContexts contexts, IValidatorKey<ObjectGraphValidator> validator)
	{
		_contexts  = contexts;
		_validator = validator;
	}

	/// <inheritdoc />
	protected override ValidationResult? IsValid(object? value, ValidationContext context)
	{
		var validator = _validator.Get(context);
		if (validator != null)
		{
			var validation = _contexts.Get(context);
			if (context.MemberName != null)
			{
				validation.Execute(context.MemberName);
			}

			validator.Validate(value, validation);
		}


		return ValidationResult.Success;
	}
}