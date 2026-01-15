using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using DragonSpark.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class Sign : Formatter<ClaimsIdentity>, ISign
{
    public Sign(ExceptionAwareIdentitySecurityDescriptor descriptor)
        : base(descriptor.Then().Select(IdentityTokenFormatter.Default)) {}
}

// TODO
sealed class WebTokenHandler : Instance<JsonWebTokenHandler>
{
    public static WebTokenHandler Default { get; } = new();

    WebTokenHandler() : base(new()) {}
}

public interface IToken : IFormatter<ClaimsSecurityDescriptorInput>;

sealed class Token : IToken
{
    readonly ISelect<ClaimsSecurityDescriptorInput, SecurityTokenDescriptor> _descriptor;
    readonly JsonWebTokenHandler                                             _handler;

    public Token(ClaimsSecurityDescriptor descriptor) : this(descriptor, WebTokenHandler.Default) {}

    public Token(ClaimsSecurityDescriptor descriptor, JsonWebTokenHandler handler)
    {
        _descriptor = descriptor;
        _handler    = handler;
    }

    public string Get(ClaimsSecurityDescriptorInput parameter)
    {
        var descriptor = _descriptor.Get(parameter);
        var result     = _handler.CreateToken(descriptor);
        return result;
    }
}

public readonly record struct ClaimsSecurityDescriptorInput(
    IDictionary<string, object> Claims,
    TimeSpan? Expiration = null)
{
    public ClaimsSecurityDescriptorInput(ClaimsIdentity identity, TimeSpan? expiration = null)
        : this(identity.Claims, expiration) {}

    public ClaimsSecurityDescriptorInput(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        : this(claims.ToDictionary(x => x.Type, object (x) => x.Value), expiration) {}
}

public sealed class ClaimsSigningCredentials : BearerSigningCredentialsBase
{
    public ClaimsSigningCredentials(BearerSettings settings) : base(settings, SecurityAlgorithms.HmacSha256) {}
}

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
        return new()
        {
            Claims             = claims,
            Issuer             = _settings.Issuer,
            Audience           = _settings.Audience,
            Expires            = _time.Get().Add(expiration ?? _settings.Window).DateTime,
            SigningCredentials = _credentials
        };
    }
}