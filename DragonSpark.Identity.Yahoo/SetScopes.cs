using AspNet.Security.OAuth.Yahoo;
using DragonSpark.Application.Security.Identity.Authentication;

namespace DragonSpark.Identity.Yahoo;

public class SetScopes : SetScopes<YahooAuthenticationOptions>
{
	protected SetScopes(params string[] scopes) : base(scopes) {}
}