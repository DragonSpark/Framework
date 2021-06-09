using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using DragonSpark.Text;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security
{
	public sealed class ExternalIdentityFormatter : Formatter<ExternalLoginInfo>
	{
		public static ExternalIdentityFormatter Default { get; } = new ExternalIdentityFormatter();

		ExternalIdentityFormatter()
			: base(ExternalLoginIdentity.Default.Then().Select(IdentityFormatter.Default).Get()) {}
	}
}