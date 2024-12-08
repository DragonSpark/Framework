using DragonSpark.Application.Components.Validation;

namespace DragonSpark.Application.AspNet.Components.Validation;

sealed class ValidatorKey : ValidatorKey<ObjectGraphValidator>
{
	public static ValidatorKey Default { get; } = new();

	ValidatorKey() : base(new object()) {}
}