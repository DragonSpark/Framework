using System.Net;

namespace DragonSpark.Application.Communication;

public sealed class CookieHeaderName : HeaderName
{
	public static CookieHeaderName Default { get; } = new();

	CookieHeaderName() : base(HttpRequestHeader.Cookie) {}
}