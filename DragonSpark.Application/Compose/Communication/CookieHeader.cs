using System.Net;

namespace DragonSpark.Application.Compose.Communication;

public sealed class CookieHeader : Header
{
	public static CookieHeader Default { get; } = new();

	CookieHeader() : base(HttpRequestHeader.Cookie) {}
}