using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class ComposePasskeyCreationOptions<T> : IComposePasskeyCreationOptions<T> where T : class
{
    readonly PasskeySettings _settings;

    public ComposePasskeyCreationOptions(PasskeySettings settings) => _settings = settings;

    public async ValueTask<string> Get(ComposePasskeyCreationOptionsInput<T> parameter)
    {
        var (context, signIn, users, subject) = parameter;
        var userId   = await users.GetUserIdAsync(subject).Off();
        var userName = await users.GetUserNameAsync(subject).Off() ?? "User";
        var entity   = new PasskeyUserEntity { Id = userId, Name = userName, DisplayName = userName };
        var options  = await signIn.MakePasskeyCreationOptionsAsync(entity).Off();
        var replace  = _settings.Host ?? context.Request.Host.Host;
        var result = options.Replace(@"""id"":""localhost""", $@"""id"":""{replace}""")
                            .Replace(@"""name"":""localhost""", $@"""name"":""{_settings.Name}""");
        return result;
    }
}