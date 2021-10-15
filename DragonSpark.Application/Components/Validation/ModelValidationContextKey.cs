namespace DragonSpark.Application.Components.Validation;

sealed class ModelValidationContextKey : ValidatorKey<GraphValidationContext>
{
	public static ModelValidationContextKey Default { get; } = new ModelValidationContextKey();

	ModelValidationContextKey() : base(new object()) {}
}