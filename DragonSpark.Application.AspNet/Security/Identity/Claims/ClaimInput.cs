using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

public readonly record struct ClaimInput(ClaimsPrincipal Owner, Claim Subject);