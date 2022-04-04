using System.Net;

namespace DragonSpark.Application.Compose.Communication;

public sealed class AuthorizationHeader : Header
{
	public static AuthorizationHeader Default { get; } = new();

	AuthorizationHeader() : base(HttpRequestHeader.Authorization) {}
}