namespace DragonSpark.Application.Communication;

public sealed class UserAgentHeader : Header
{
	public static UserAgentHeader Default { get; } = new();

	UserAgentHeader() : base("User-Agent") {}
}