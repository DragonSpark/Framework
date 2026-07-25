using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class ComposePasskeyCreationOptions<T> : IComposePasskeyCreationOptions<T> where T : class
{
    public static ComposePasskeyCreationOptions<T> Default { get; } = new();

    ComposePasskeyCreationOptions() {}

    public async ValueTask<string> Get(ComposePasskeyCreationOptionsInput<T> parameter)
    {
        var (signIn, users, subject) = parameter;
        var userId   = await users.GetUserIdAsync(subject).Off();
        var userName = await users.GetUserNameAsync(subject).Off() ?? "User";
        var entity   = new PasskeyUserEntity { Id = userId, Name = userName, DisplayName = userName };
        var result  = await signIn.MakePasskeyCreationOptionsAsync(entity).Off();
        return result;
    }
}