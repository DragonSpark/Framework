using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public readonly record struct CurrentProfileStateInput(ClaimsPrincipal Principal, IdentityUser? User);