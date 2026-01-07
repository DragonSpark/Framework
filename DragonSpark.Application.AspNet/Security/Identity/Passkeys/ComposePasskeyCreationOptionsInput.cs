using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public readonly record struct ComposePasskeyCreationOptionsInput<T>(
    HttpContext Context,
    SignInManager<T> SignIn,
    UserManager<T> User,
    T Subject) where T : class
{
    public ComposePasskeyCreationOptionsInput(HttpContext Context, SignInManager<T> SignIn, T Subject)
        : this(Context, SignIn, SignIn.UserManager, Subject) {}
}