using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Application.Security.Identity.Claims.Policy;

sealed class RequireClaim : ICommand<AuthorizationPolicyBuilder>
{
	readonly string _claim;

	public RequireClaim(string claim) => _claim = claim;

	public void Execute(AuthorizationPolicyBuilder parameter)
	{
		parameter.RequireClaim(_claim);
	}
}