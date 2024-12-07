using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Application.Security.Identity.Claims.Policy;

sealed class RequireUserNames : ICommand<AuthorizationPolicyBuilder>
{
	readonly Array<string> _names;

	public RequireUserNames(params string[] names) => _names = names;

	public void Execute(AuthorizationPolicyBuilder parameter)
	{
		parameter.Requirements.Add(new UserNamesRequirement(_names));
	}
}