using AspNet.Security.OAuth.Patreon;
using DragonSpark.Application.Security.Identity.Authentication;

namespace DragonSpark.Identity.Patreon
{
	public class SetPatreonScopes : SetScopes<PatreonAuthenticationOptions>
	{
		protected SetPatreonScopes(params string[] scopes) : base(scopes) {}
	}
}