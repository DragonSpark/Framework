using System.Net;

namespace DragonSpark.Application.Connections;

public sealed class AuthorizationHeaderName : HeaderName
{
	public static AuthorizationHeaderName Default { get; } = new();

	AuthorizationHeaderName() : base(HttpRequestHeader.Authorization) {}
}