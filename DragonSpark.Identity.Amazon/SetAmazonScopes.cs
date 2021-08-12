using AspNet.Security.OAuth.Amazon;
using DragonSpark.Application.Security.Identity.Authentication;

namespace DragonSpark.Identity.Amazon
{
	public class SetAmazonScopes : SetScopes<AmazonAuthenticationOptions>
	{
		protected SetAmazonScopes(params string[] scopes) : base(scopes) {}
	}
}