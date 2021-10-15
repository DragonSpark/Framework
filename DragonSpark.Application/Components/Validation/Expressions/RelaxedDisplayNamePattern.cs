namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class RelaxedDisplayNamePattern : Expression
{
	public const string Expression = "[a-zA-Z0-9- _.*@!]";

	public static RelaxedDisplayNamePattern Default { get; } = new RelaxedDisplayNamePattern();

	RelaxedDisplayNamePattern() : base(Expression) {}
}