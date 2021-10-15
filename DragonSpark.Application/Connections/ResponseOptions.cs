using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.ResponseCompression;
using System.Linq;

namespace DragonSpark.Application.Connections;

public sealed class ResponseOptions : ICommand<ResponseCompressionOptions>
{
	public static ResponseOptions Default { get; } = new ResponseOptions();

	ResponseOptions() : this(ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" })
	                                                    .Result()) {}

	readonly Array<string> _types;

	public ResponseOptions(Array<string> types) => _types = types;

	public void Execute(ResponseCompressionOptions parameter)
	{
		parameter.MimeTypes = _types.Open();
	}
}