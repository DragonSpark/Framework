using DragonSpark.Application.AspNet.Communication;

namespace DragonSpark.Azure.Storage.Uploads;

sealed class ContentTypeHeader : Header
{
	public static ContentTypeHeader Default { get; } = new();

	ContentTypeHeader() : base(ReportedContentTypeHeader.Default) {}
}