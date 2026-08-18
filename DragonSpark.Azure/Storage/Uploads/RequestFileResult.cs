namespace DragonSpark.Azure.Storage.Uploads;

public sealed class RequestFileResult : RequestFileResultBase
{
	public static RequestFileResult Default { get; } = new();

	RequestFileResult() {}
}