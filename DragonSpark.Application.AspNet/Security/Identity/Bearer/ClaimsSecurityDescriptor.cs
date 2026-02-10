using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class ClaimsSecurityDescriptor : ISelect<ClaimsSecurityDescriptorInput, SecurityTokenDescriptor>
{
    readonly BearerSettings     _settings;
    readonly SigningCredentials _credentials;
    readonly ITime              _time;

    public ClaimsSecurityDescriptor(BearerSettings settings, ClaimsSigningCredentials credentials)
        : this(settings, credentials.Get(), Time.Default) {}

    public ClaimsSecurityDescriptor(BearerSettings settings, SigningCredentials credentials, ITime time)
    {
        _settings    = settings;
        _credentials = credentials;
        _time        = time;
    }

    public SecurityTokenDescriptor Get(ClaimsSecurityDescriptorInput parameter)
    {
        var (claims, expiration) = parameter;
        var now = _time.Get();
        return new()
        {
            Claims             = claims,
            Issuer             = _settings.Issuer,
            Audience           = _settings.Audience,
            NotBefore          = now.UtcDateTime,
            IssuedAt           = now.UtcDateTime,
            Expires            = now.Add(expiration ?? _settings.Window).UtcDateTime,
            SigningCredentials = _credentials
        };
    }
}