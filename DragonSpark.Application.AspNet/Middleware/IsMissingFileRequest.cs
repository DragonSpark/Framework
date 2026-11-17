using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware;

sealed class IsMissingFileRequest : ICondition<HttpRequest>
{
	public static IsMissingFileRequest Default { get; } = new();

	IsMissingFileRequest() {}

	public bool Get(HttpRequest parameter)
		=> parameter.HttpContext.GetEndpoint() is null // TODO: Remove this?
		   && !parameter.Path.StartsWithSegments("/_framework")
		   && !parameter.Path.StartsWithSegments("/_vs")
		   && (HttpMethods.IsGet(parameter.Method) || HttpMethods.IsHead(parameter.Method))
		   && parameter.Path.Value.NullIfEmpty() is {} path
		   && HasFileExtension(path);

	static bool HasFileExtension(string path)
	{
		var lastSlash = path.LastIndexOf('/');
		var lastDot   = path.LastIndexOf('.');
		return lastDot > lastSlash && lastDot < path.Length - 1;
	}
}