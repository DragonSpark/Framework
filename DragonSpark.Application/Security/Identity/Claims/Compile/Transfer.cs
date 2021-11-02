using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Compile;

public readonly record struct Transfer(ClaimsPrincipal From, ClaimsPrincipal To);