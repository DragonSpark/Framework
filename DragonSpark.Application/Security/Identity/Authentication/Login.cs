using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public readonly record struct Login(ClaimsPrincipal Identity, string Provider, string Key);