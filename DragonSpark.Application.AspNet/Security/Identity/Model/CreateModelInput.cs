using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public readonly record struct CreateModelInput(ClaimsPrincipal Principal, string Address);