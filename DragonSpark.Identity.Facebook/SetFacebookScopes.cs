using DragonSpark.Application.Security.Identity.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace DragonSpark.Identity.Facebook
{
	public class SetFacebookScopes : SetScopes<FacebookOptions>
	{
		protected SetFacebookScopes(params string[] scopes) : base(scopes) {}
	}
}