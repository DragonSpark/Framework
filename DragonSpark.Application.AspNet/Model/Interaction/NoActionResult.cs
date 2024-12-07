namespace DragonSpark.Application.Model.Interaction;

public sealed class NoActionResult : SuccessResultBase
{
	public static NoActionResult Default { get; } = new();

	NoActionResult() {}
}