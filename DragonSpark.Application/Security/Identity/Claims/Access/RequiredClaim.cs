using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Access
{
	public class RequiredClaim : IRequiredClaim
	{
		readonly string _claim;

		protected RequiredClaim(string claim) => _claim = claim;

		public string Get(ClaimsPrincipal parameter)
			=> parameter.FindFirstValue(_claim) ??
			   throw new
				   InvalidOperationException($"Content not found for claim '{_claim}' in user {parameter.UserName()}.");
	}
}