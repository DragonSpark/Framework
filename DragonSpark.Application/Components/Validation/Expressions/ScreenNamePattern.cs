namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class ScreenNamePattern : Expression
{
	public static ScreenNamePattern Default { get; } = new ScreenNamePattern();

	ScreenNamePattern() : base("[a-zA-Z0-9_]") {}
}