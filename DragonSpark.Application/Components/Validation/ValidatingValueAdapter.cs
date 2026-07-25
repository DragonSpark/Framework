using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Components.Validation;

sealed class ValidatingValueAdapter<T> : IValidatingValue<T>
{
	readonly IValidateValue<T> _validate;

	public ValidatingValueAdapter(IValidateValue<T> validate) => _validate = validate;

	public ValueTask<bool> Get(Stop<T> parameter) => _validate.Get(parameter).ToOperation();
}