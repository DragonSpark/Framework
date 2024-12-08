using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

sealed class DisplayNameClaim : IDisplayNameClaim
{
	public static DisplayNameClaim Default { get; } = new();

	DisplayNameClaim() {}

	public string Get(string parameter) => ClaimTypes.Name;
}