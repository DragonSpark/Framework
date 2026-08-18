using DragonSpark.Application.AspNet.Communication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public class NewUploadBase : INewUpload
{
	readonly RequestHeader               _header;
	readonly IStopAware<string, string?> _type;
	readonly string                      _name;

	public NewUploadBase(RequestHeader header, IStopAware<string, string?> type)
		: this(header, type, ReportedContentTypeHeader.Default) {}

	public NewUploadBase(RequestHeader header, IStopAware<string, string?> type, string name)
	{
		_header = header;
		_type = type;
		_name   = name;
	}

	public async ValueTask<ICollection<object>> Get(Stop<FileRequest> parameter)
	{
		var ((name, contentType), stop) = parameter;
		var result = _header.Get();
		var type = contentType.NullIfEmpty() ?? await _type.Off(new(Path.GetExtension(name), stop)) ??
				   throw new InvalidOperationException($"Could not determine content type for {name}");
		result.Add(new Dictionary<string, string> { [_name] = type });
		return result;
	}
}