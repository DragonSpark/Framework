using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public readonly record struct ComposePasskeyCreationOptionsInput<T>(
    SignInManager<T> SignIn,
    UserManager<T> User,
    T Subject) where T : class
{
    public ComposePasskeyCreationOptionsInput(SignInManager<T> SignIn, T Subject)
        : this(SignIn, SignIn.UserManager, Subject) {}
}