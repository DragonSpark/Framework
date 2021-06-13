using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Model
{
	public sealed class ExternalLoginIdentity : Select<ExternalLoginInfo, ProviderIdentity>
	{
		public static ExternalLoginIdentity Default { get; } = new ExternalLoginIdentity();

		ExternalLoginIdentity() : base(x => new ProviderIdentity(x.LoginProvider, x.ProviderKey)) {}
	}
}