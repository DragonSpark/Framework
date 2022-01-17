namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class ScreenNameExtendedPattern : Expression
{
	public static ScreenNameExtendedPattern Default { get; } = new ();

	ScreenNameExtendedPattern() : base("[a-zA-Z0-9_-]") {}
}