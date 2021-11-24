namespace DragonSpark.Presentation.Interaction;

public sealed record InvalidResult : IInteractionResult
{
	public static InvalidResult Default { get; } = new();

	InvalidResult() {}
}