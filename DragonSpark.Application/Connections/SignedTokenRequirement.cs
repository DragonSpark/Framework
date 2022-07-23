using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections;

sealed class SignedTokenRequirement : AuthorizationHandler<SignedTokenRequirement, HttpContext>,
                                      IAuthorizationRequirement
{
	readonly IsSigned _signed;

	public SignedTokenRequirement(IsSigned signed) => _signed = signed;

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
	                                               SignedTokenRequirement requirement,
	                                               HttpContext resource)
	{
		var signed = _signed.Get(resource);
		if (signed)
		{
			context.Succeed(requirement);
		}
		else
		{
			context.Fail();
		}

		return Task.CompletedTask;
	}
}