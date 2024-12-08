using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

public readonly record struct CreateUserResult<T>(T User, IdentityResult Result);