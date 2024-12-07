namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class ValidUrlPathCharacter : Expression
{
	public static ValidUrlPathCharacter Default { get; } = new();

	ValidUrlPathCharacter() : base("[A-Za-z0-9._-]") {}
}