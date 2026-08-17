namespace DragonSpark.Azure.Storage.Uploads;

public sealed class SourceFileRequest : RequestFileResultBase // TODO
{
	public static SourceFileRequest Default { get; } = new();

	SourceFileRequest() : base(x => x.IsStreamable) {}
}