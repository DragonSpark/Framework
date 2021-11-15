namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ValidationMessage<T> : IValidationMessage<T>
{
	public static ValidationMessage<T> Default { get; } = new();

	ValidationMessage() {}

	public string? Get(T? parameter) => null;
}