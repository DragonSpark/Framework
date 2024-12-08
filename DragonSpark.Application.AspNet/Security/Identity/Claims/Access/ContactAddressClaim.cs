using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Access;

public sealed class ContactAddressClaim : ReadClaim
{
	public static ContactAddressClaim Default { get; } = new();

	ContactAddressClaim() : base(ClaimTypes.Email) {}
}