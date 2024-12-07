using DragonSpark.Application.Security;

namespace DragonSpark.Application.Communication;

public sealed class CurrentCookie : CurrentHeader
{
	public CurrentCookie(ICurrentContext context) : base(context, CookieHeader.Default) {}
}