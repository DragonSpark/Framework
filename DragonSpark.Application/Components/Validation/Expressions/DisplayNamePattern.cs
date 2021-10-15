namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class DisplayNamePattern : Expression
{
	public const string Expression = "[a-zA-Z0-9- _]";

	public static DisplayNamePattern Default { get; } = new DisplayNamePattern();

	DisplayNamePattern() : base(Expression) {}
}