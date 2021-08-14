using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace DragonSpark.Application.Security.Identity.Claims.Policy
{
	sealed class RequireClaims : ICommand<AuthorizationPolicyBuilder>
	{
		readonly Array<string> _claims;

		public RequireClaims(params string[] claims) => _claims = claims;

		public void Execute(AuthorizationPolicyBuilder parameter)
		{
			_claims.Open().Aggregate(parameter, (builder, claim) => builder.RequireClaim(claim));
		}
	}
}