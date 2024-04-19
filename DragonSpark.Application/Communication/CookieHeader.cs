namespace DragonSpark.Application.Communication;

public sealed class CookieHeader : Header
{
	public static CookieHeader Default { get; } = new();

	CookieHeader() : base(CookieHeaderName.Default) {}
}