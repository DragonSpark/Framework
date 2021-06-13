using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class DisplayNameClaim : IDisplayNameClaim
	{
		public static DisplayNameClaim Default { get; } = new DisplayNameClaim();

		DisplayNameClaim() {}

		public string Get(string parameter) => ClaimTypes.Name;
	}
}