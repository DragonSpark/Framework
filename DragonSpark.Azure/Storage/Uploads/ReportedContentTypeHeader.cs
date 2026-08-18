namespace DragonSpark.Azure.Storage.Uploads;

public sealed class ReportedContentTypeHeader : Text.Text
{
	public static ReportedContentTypeHeader Default { get; } = new();

	ReportedContentTypeHeader() : base("x-dragonspark-content-type") {}
}