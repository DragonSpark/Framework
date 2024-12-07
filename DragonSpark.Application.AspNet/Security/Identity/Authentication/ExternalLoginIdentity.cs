using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class ExternalLoginIdentity : Select<ExternalLoginInfo, ProviderIdentity>
{
	public static ExternalLoginIdentity Default { get; } = new();

	ExternalLoginIdentity() : base(x => new ProviderIdentity(x.LoginProvider, x.ProviderKey)) {}
}