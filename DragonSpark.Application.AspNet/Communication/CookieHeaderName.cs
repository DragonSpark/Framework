using System.Net;

namespace DragonSpark.Application.AspNet.Communication;

public sealed class CookieHeaderName : HeaderName
{
	public static CookieHeaderName Default { get; } = new();

	CookieHeaderName() : base(HttpRequestHeader.Cookie) {}
}