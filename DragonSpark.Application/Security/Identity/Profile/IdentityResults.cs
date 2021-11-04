using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class IdentityResults : ISelect<SignInResult, IdentityResult>
{
	public static IdentityResults Default { get; } = new();

	IdentityResults() {}

	public IdentityResult Get(SignInResult parameter)
		=> parameter.Succeeded
			   ? IdentityResult.Success
			   : IdentityResult.Failed(new IdentityError
			   {
				   Description = parameter.IsLockedOut
					                 ? "User is Locked Out"
					                 : parameter.IsNotAllowed
						                 ? "Authentication is Not Allowed for this user"
						                 : "Two Factor Authentication is Required"
			   });
}