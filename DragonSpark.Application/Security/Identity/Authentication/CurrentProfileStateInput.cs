using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public readonly record struct CurrentProfileStateInput(ClaimsPrincipal Principal, IdentityUser? User);