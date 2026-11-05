namespace DragonSpark.Azure.Storage;

public sealed class RequestFileResult : RequestFileResultBase
{
	public static RequestFileResult Default { get; } = new();

	RequestFileResult() {}
}