namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class InvalidContext
{
	public static InvalidContext Default { get; } = new();

	InvalidContext() {}

	[Invalid]
	public string Invalid { get; set; } = default!;
}