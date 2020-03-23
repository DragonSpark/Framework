using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class AppliedPrincipal : IAppliedPrincipal
	{
		readonly IProviderDefinitions _definitions;

		[UsedImplicitly]
		public AppliedPrincipal(IProviderDefinitions definitions) => _definitions = definitions;

		public ClaimsPrincipal Get(ExternalLoginInfo parameter)
		{
			if (_definitions.Condition.Get(parameter.LoginProvider))
			{
				var claims = _definitions.Get(parameter.LoginProvider)
				                         .ClaimMappings.Open()
				                         .Get(parameter)
				                         .Where(x => x != null)
				                         .ToArray();

				var identity = new ClaimsIdentity(claims);
				var result   = new ClaimsPrincipal(parameter.Principal.Identities.Append(identity).Open());
				return result;
			}

			return parameter.Principal;
		}
	}
}