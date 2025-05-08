namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class KeywordsPattern : Expression
{
	public static KeywordsPattern Default { get; } = new();

	KeywordsPattern() : base("[a-zA-Z0-9_, ]") {}
}