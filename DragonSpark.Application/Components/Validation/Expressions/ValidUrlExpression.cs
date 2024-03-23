using JetBrains.Annotations;

namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class ValidUrlExpression : Expression
{
	[UsedImplicitly]
	public static ValidUrlExpression Default { get; } = new();

	ValidUrlExpression() : base(@"^(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png)$") {}
}