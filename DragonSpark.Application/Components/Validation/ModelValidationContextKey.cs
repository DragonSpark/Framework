namespace DragonSpark.Application.Components.Validation
{
	sealed class ModelValidationContextKey : ValidatorKey<ModelValidationContext>
	{
		public static ModelValidationContextKey Default { get; } = new ModelValidationContextKey();

		ModelValidationContextKey() : base(new object()) {}
	}
}