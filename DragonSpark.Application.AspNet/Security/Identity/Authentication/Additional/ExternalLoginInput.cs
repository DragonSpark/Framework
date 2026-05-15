using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public readonly record struct ExternalLoginInput(ExternalLoginInfo Subject, bool Persist);