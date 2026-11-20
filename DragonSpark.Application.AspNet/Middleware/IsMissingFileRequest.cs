using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware;

sealed class IsMissingFileRequest : ICondition<HttpRequest>
{
	public static IsMissingFileRequest Default { get; } = new();

	IsMissingFileRequest() : this(HasFileExtension.Default) {}

	readonly ICondition<string> _extension;

	public IsMissingFileRequest(ICondition<string> extension) => _extension = extension;

	public bool Get(HttpRequest parameter)
		=> parameter.HttpContext.GetEndpoint() is null
		   && (HttpMethods.IsGet(parameter.Method) || HttpMethods.IsHead(parameter.Method))
		   && parameter.Path.Value.NullIfEmpty() is {} path
		   && _extension.Get(path);
}