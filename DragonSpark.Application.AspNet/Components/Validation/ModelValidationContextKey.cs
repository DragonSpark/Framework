using DragonSpark.Application.Components.Validation;

namespace DragonSpark.Application.AspNet.Components.Validation;

sealed class ModelValidationContextKey : ValidatorKey<GraphValidationContext>
{
	public static ModelValidationContextKey Default { get; } = new();

	ModelValidationContextKey() : base(new object()) {}
}