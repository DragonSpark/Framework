using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

public readonly record struct SignInInput<T>(T User, string Subject, string Scheme)
{
    public SignInInput(T User, string Subject) : this(User, Subject, IdentityConstants.ApplicationScheme) {}
}