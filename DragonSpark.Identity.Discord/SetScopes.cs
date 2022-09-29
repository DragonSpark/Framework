using AspNet.Security.OAuth.Discord;
using DragonSpark.Application.Security.Identity.Authentication;

namespace DragonSpark.Identity.Discord;

public class SetScopes : SetScopes<DiscordAuthenticationOptions>
{
	protected SetScopes(params string[] scopes) : base(scopes) {}
}