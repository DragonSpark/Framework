using DragonSpark.Compose;
using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation
{
	public sealed class ValidationContexts : IValidationContexts
	{
		readonly IValidatorKey<ModelValidationContext> _context;
		readonly IValidatorKey<ObjectGraphValidator>   _validator;
		public static ValidationContexts Default { get; } = new ValidationContexts();

		ValidationContexts() : this(ModelValidationContextKey.Default, ValidatorKey.Default) {}

		public ValidationContexts(IValidatorKey<ModelValidationContext> context,
		                          IValidatorKey<ObjectGraphValidator> validator)
		{
			_context   = context;
			_validator = validator;
		}

		public ValidationContext Get(NewValidationContext parameter)
		{
			var (field, validator, context) = parameter;
			var result = new ValidationContext(field.Model) { MemberName = field.FieldName };

			_context.Assign(result, context);
			_validator.Assign(result, validator);
			return result;
		}

		public ModelValidationContext Get(ValidationContext parameter)
			=> _context.Get(parameter) ?? throw new InvalidOperationException();
	}
}