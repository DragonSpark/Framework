namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class IdentifierPattern : Expression
{
	public const string Expression = "[a-zA-Z0-9-_]";

	public static IdentifierPattern Default { get; } = new IdentifierPattern();

	IdentifierPattern() : base(Expression) {}
}