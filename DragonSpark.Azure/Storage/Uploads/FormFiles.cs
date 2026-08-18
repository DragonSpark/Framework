using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Azure.Storage.Uploads;

public sealed class FormFiles : ISelect<FormFileInput, IFormFile>
{
	public static FormFiles Default { get; } = new();

	FormFiles() : this(ContentTypeHeader.Default) {}

	readonly ISelect<IHeaderDictionary, string?> _header;

	public FormFiles(ISelect<IHeaderDictionary, string?> header) => _header = header;

	public IFormFile Get(FormFileInput parameter)
	{
		var (header, previous) = parameter;
		var result = new ReportedContentTypeAwareFormFile(previous, _header.Get(header).Verify());
		return result;
	}
}