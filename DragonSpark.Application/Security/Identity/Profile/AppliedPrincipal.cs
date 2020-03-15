using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Profile {
	public class AppliedPrincipal : IAppliedPrincipal
	{
		readonly IReadOnlyDictionary<string, string> _claims;

		public AppliedPrincipal(IReadOnlyDictionary<string, string> claims) => _claims = claims;

		public ClaimsPrincipal Get(ExternalLoginInfo parameter)
		{
			var key        = _claims.TryGetValue(parameter.LoginProvider, out var value) ? value : ClaimTypes.Name;
			var claim      = DisplayName.Default.Claim(parameter.Principal.FindFirst(key));
			var identity   = new ClaimsIdentity(claim.Yield());
			var identities = parameter.Principal.Identities.Append(identity);
			var result     = new ClaimsPrincipal(identities);
			return result;
		}
	}
}