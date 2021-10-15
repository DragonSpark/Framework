namespace DragonSpark.Presentation.Interaction;

public sealed class SuccessResult : SuccessResultBase
{
	public static SuccessResult Default { get; } = new();

	SuccessResult() {}
}