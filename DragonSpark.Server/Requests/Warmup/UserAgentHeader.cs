using DragonSpark.Application.Communication;

namespace DragonSpark.Server.Requests.Warmup;

sealed class UserAgentHeader : Header
{
	public static UserAgentHeader Default { get; } = new();

	UserAgentHeader() : base("User-Agent") { }
}