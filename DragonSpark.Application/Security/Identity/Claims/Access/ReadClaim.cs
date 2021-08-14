using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Access
{
	public class ReadClaim : IReadClaim
	{
		readonly string _claim;

		public ReadClaim(string claim) => _claim = claim;

		public Accessed Get(ClaimsPrincipal parameter)
		{
			var exists = parameter.HasClaim(_claim);
			var result = exists
				             ? new Accessed(_claim, true, parameter.FindFirstValue(_claim))
				             : new Accessed(_claim, false, null);
			return result;
		}
	}
}