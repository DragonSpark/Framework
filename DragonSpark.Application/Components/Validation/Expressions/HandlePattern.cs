namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class HandlePattern : Expression
{
	public static HandlePattern Default { get; } = new HandlePattern();

	HandlePattern() : base("[a-zA-Z0-9_]") {}
}