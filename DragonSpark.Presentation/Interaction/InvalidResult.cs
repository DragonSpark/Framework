namespace DragonSpark.Presentation.Interaction;

public sealed class InvalidResult : UnsuccessfulResultBase
{
	public static InvalidResult Default { get; } = new();

	InvalidResult() {}
}