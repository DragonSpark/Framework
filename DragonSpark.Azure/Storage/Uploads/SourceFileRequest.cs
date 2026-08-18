namespace DragonSpark.Azure.Storage.Uploads;

public sealed class SourceFileRequest : RequestFileResultBase
{
	public static SourceFileRequest Default { get; } = new();

	SourceFileRequest() : base(x => x.IsStreamable) {}
}