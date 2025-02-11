using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims;

public sealed class ContactAddressClaim : ReadClaim
{
	public static ContactAddressClaim Default { get; } = new();

	ContactAddressClaim() : base(ClaimTypes.Email) {}
}