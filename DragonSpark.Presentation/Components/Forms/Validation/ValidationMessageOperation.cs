using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class ValidationMessageOperation<T> : IOperation<ValidationContext>
{
	readonly IValidationMessage<T> _validator;

	public ValidationMessageOperation(IValidationMessage<T> validator) => _validator = validator;

	public ValueTask Get(ValidationContext parameter)
	{
		var ((_, field), messages, _) = parameter;
		var message = _validator.Get(field.GetValue<T>());
		if (!string.IsNullOrEmpty(message))
		{
			messages.Add(in field, message);
		}
		return ValueTask.CompletedTask;
	}
}