using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

public class UserClaimsPrincipals<T> : UserClaimsPrincipalFactory<T> where T : class
{
	readonly string _applicationName;

	public UserClaimsPrincipals(UserManager<T> userManager, IOptions<IdentityOptions> optionsAccessor,
	                            string applicationName = "Identity.Application")
		: base(userManager, optionsAccessor)
		=> _applicationName = applicationName;

	protected override async Task<ClaimsIdentity> GenerateClaimsAsync(T user)
	{
		var userId   = await UserManager.GetUserIdAsync(user).Await();
		var userName = await UserManager.GetUserNameAsync(user).Await();
		var result = new ClaimsIdentity(_applicationName, Options.ClaimsIdentity.UserNameClaimType,
		                                Options.ClaimsIdentity.RoleClaimType);
		result.AddClaim(new(ClaimTypes.NameIdentifier, userId));
		result.AddClaim(new(ClaimTypes.Name, userName.Verify()));
		if (UserManager.SupportsUserSecurityStamp)
		{
			result.AddClaim(new(Options.ClaimsIdentity.SecurityStampClaimType,
			                    await UserManager.GetSecurityStampAsync(user).Await()));
		}

		if (UserManager.SupportsUserClaim)
		{
			result.AddClaims(await UserManager.GetClaimsAsync(user).Await());
		}

		return result;
	}
}