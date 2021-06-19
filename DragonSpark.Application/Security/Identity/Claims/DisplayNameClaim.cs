using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims
{
	sealed class DisplayNameClaim : IDisplayNameClaim
	{
		public static DisplayNameClaim Default { get; } = new DisplayNameClaim();

		DisplayNameClaim() {}

		public string Get(string parameter) => ClaimTypes.Name;
	}
}