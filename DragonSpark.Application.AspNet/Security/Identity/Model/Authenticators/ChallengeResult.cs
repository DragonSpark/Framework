using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

public readonly record struct ChallengeResult<T>(T User, ExternalLoginInfo Information, IdentityResult Result)
	where T : IdentityUser;