using DragonSpark.Application.AspNet.Security;

namespace DragonSpark.Application.AspNet.Communication;

public sealed class CurrentCookie : CurrentHeader
{
	public CurrentCookie(ICurrentContext context) : base(context, CookieHeader.Default) {}
}