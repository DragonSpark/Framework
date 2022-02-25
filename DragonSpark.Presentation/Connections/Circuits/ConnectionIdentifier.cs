using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class ConnectionIdentifier : IConnectionIdentifier
{
	readonly IHttpContextAccessor _accessor;

	public ConnectionIdentifier(IHttpContextAccessor accessor) => _accessor = accessor;

	public string? Get() => _accessor.HttpContext?.Request.Query["id"];
}