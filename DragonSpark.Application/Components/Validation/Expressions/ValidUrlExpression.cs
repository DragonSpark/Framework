namespace DragonSpark.Application.Components.Validation.Expressions
{
	public sealed class ValidUrlExpression : Expression
	{
		public static ValidUrlExpression Default { get; } = new ValidUrlExpression();

		ValidUrlExpression() : base(@"^(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png)$") {}
	}
}