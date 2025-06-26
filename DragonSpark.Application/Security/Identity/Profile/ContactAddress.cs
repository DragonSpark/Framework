using System.Security.Claims;
using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class ContactAddress : RequiredClaim
{
    public static ContactAddress Default { get; } = new();

    ContactAddress() : base(ClaimTypes.Email) {}
}