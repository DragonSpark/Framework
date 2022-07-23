using DragonSpark.Application.Communication;

namespace DragonSpark.Server.Requests.Warmup;

sealed class MicrosoftAuthenticationHeader : Header
{
	public static MicrosoftAuthenticationHeader Default { get; } = new();

	MicrosoftAuthenticationHeader() : base("x-ms-auth-allow-http") { }
}