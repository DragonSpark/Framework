using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

public readonly record struct SignInInput<T>(T User, string Subject, bool Persistent, string Scheme)
{
    public SignInInput(T User, string Subject, bool Persistent)
        : this(User, Subject, Persistent, IdentityConstants.ApplicationScheme) {}
}