using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class EncryptedClaimsSecurityDescriptor : ISelect<ClaimsSecurityDescriptorInput, SecurityTokenDescriptor>
{
    readonly BearerSettings        _settings;
    readonly EncryptingCredentials _credentials;
    readonly ITime                 _time;

    public EncryptedClaimsSecurityDescriptor(BearerSettings settings, ClaimsEncryptionCredentials credentials)
        : this(settings, credentials.Get(), Time.Default) {}

    public EncryptedClaimsSecurityDescriptor(BearerSettings settings, EncryptingCredentials credentials, ITime time)
    {
        _settings    = settings;
        _credentials = credentials;
        _time        = time;
    }

    public SecurityTokenDescriptor Get(ClaimsSecurityDescriptorInput parameter)
    {
        var (claims, expiration) = parameter;
        var time = _time.Get();
        return new()
        {
            Claims                = claims,
            NotBefore             = time.UtcDateTime,
            Issuer                = _settings.Issuer,
            Audience              = _settings.Audience,
            Expires               = time.Add(expiration ?? _settings.Window).UtcDateTime,
            EncryptingCredentials = _credentials
        };
    }
}