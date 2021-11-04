using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Text;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity;

public sealed class ExternalIdentityFormatter : Formatter<ExternalLoginInfo>
{
	public static ExternalIdentityFormatter Default { get; } = new();

	ExternalIdentityFormatter()
		: base(ExternalLoginIdentity.Default.Then().Select(IdentityFormatter.Default).Get()) {}
}