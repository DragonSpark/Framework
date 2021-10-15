namespace DragonSpark.Presentation.Interaction;

public sealed class NoActionResult : SuccessResultBase
{
	public static NoActionResult Default { get; } = new();

	NoActionResult() {}
}