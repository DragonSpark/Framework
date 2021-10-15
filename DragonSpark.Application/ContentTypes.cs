using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.StaticFiles;

namespace DragonSpark.Application;

public sealed class ContentTypes : IAlteration<string>
{
	public static ContentTypes Default { get; } = new ContentTypes();

	ContentTypes() : this(new FileExtensionContentTypeProvider()) {}

	readonly IContentTypeProvider _provider;

	public ContentTypes(IContentTypeProvider provider) => _provider = provider;

	public string Get(string parameter)
		=> _provider.TryGetContentType(parameter, out var contentType) ? contentType : "application/octet-stream";
}
