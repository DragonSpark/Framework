using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Profile;

public readonly record struct CreateRequestResult(IdentityResult Result, IdentityUser? User);