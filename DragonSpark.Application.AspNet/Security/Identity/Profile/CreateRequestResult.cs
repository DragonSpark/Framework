using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

public readonly record struct CreateRequestResult(IdentityResult Result, IdentityUser? User);