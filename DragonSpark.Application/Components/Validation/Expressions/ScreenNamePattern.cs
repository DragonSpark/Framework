namespace DragonSpark.Application.Components.Validation.Expressions
{
	public sealed class ScreenNamePattern : Expression
	{
		public static ScreenNamePattern Default { get; } = new ScreenNamePattern();

		ScreenNamePattern() : base("[a-zA-Z0-9_]") {}
	}

	public sealed class IdentifierPattern : Expression
	{
		public const string Expression = "[a-zA-Z0-9-_]";

		public static IdentifierPattern Default { get; } = new IdentifierPattern();

		IdentifierPattern() : base(Expression) {}
	}
}