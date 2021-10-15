using DragonSpark.Model;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation;

public class ValidatorKey<T> : IValidatorKey<T> where T : class
{
	readonly object _key;

	public ValidatorKey(object key) => _key = key;

	public T? Get(ValidationContext parameter)
		=> parameter.Items.TryGetValue(_key, out var item) && item is T result ? result : default;

	public void Execute(Pair<ValidationContext, T> parameter)
	{
		var (key, value) = parameter;

		key.Items[_key] = value;
	}
}

sealed class ValidatorKey : ValidatorKey<ObjectGraphValidator>
{
	public static ValidatorKey Default { get; } = new ValidatorKey();

	ValidatorKey() : base(new object()) {}
}