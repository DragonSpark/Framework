using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public class ExternalLogin<T> : IExternalLogin where T : class
{
    readonly IAuthentications<T> _sessions;
    readonly ILogger             _logger;

    protected ExternalLogin(IAuthentications<T> sessions, ILogger logger)
    {
        _sessions = sessions;
        _logger   = logger;
    }

    public async ValueTask<SignInResult> Get(Stop<ExternalLoginInfo> parameter)
    {
        var (subject, _) = parameter;
        using var session = _sessions.Get();
        var result = await session.Subject.ExternalLoginSignInAsync(subject.LoginProvider, subject.ProviderKey,
                                                                    isPersistent: false, bypassTwoFactor: true)
                                  .Off();

        if (result.Succeeded)
        {
            _logger.LogInformation("{Name} logged in with {LoginProvider} provider", subject.Principal.Identity?.Name,
                                   subject.LoginProvider);
        }

        return result;
    }
}