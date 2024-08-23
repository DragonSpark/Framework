using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Claims.Policy;

public sealed class UserNamesHandler : AuthorizationHandler<UserNamesRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
	                                               UserNamesRequirement requirement)
	{
		if (context.User.Identity?.Name != null && requirement.Get(context.User.Identity.Name))
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}