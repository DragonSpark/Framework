using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity;

public readonly record struct ProviderIdentity(string Provider, string Name, string Identity)
{
	public static implicit operator ProviderIdentity(ExternalLoginInfo instance) => instance.AsIdentity();

	public ProviderIdentity(string Provider, string Identity)
		: this(Provider, Provider == "Twitter" ? "X" : Provider, Identity) {} // TODO: Get this passed in legitimately
}