using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Communication.Http;

public class RefreshToken<T> : IRefreshToken where T : class
{
    readonly IAuthentications<T>                     _authentications;
    readonly ISecureDataFormat<AuthenticationTicket> _format;
    readonly ITime                                   _time;

    protected RefreshToken(IAuthentications<T> authentications, IOptionsMonitor<BearerTokenOptions> options)
        : this(authentications, options.Get(IdentityConstants.BearerScheme).RefreshTokenProtector, Time.Default) {}

    protected RefreshToken(IAuthentications<T> authentications,
                           ISecureDataFormat<AuthenticationTicket> format, ITime time)
    {
        _authentications = authentications;
        _format          = format;
        _time            = time;
    }

    public async ValueTask<IResult> Get(Stop<string> parameter)
    {
        using var authentication = _authentications.Get();
        var       ticket         = _format.Unprotect(parameter);

        if (ticket?.Properties.ExpiresUtc is {} expiresUtc && _time.Get().UtcDateTime < expiresUtc &&
            await authentication.Subject.ValidateSecurityStampAsync(ticket.Principal).Off() is {} user)
        {
            var principal = await authentication.Subject.CreateUserPrincipalAsync(user).Off();
            return TypedResults.SignIn(principal, authenticationScheme: IdentityConstants.BearerScheme);
        }

        return TypedResults.NoContent();
    }
}