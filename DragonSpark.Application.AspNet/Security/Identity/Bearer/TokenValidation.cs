using System;
using System.Collections.Generic;
using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Composition;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using DragonSpark.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public sealed class TokenValidation : Instance<TokenValidationParameters>
{
    public TokenValidation(BearerSettings settings) : this(settings, EncodedTextAsData.Default.Get(settings.Key)) {}

    [Candidate(false)]
    public TokenValidation(BearerSettings settings, byte[] key)
        : base(new()
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = settings.Issuer,
            ValidAudience            = settings.Audience,
            AuthenticationType       = IdentityConstants.ApplicationScheme,
            IssuerSigningKey         = new SymmetricSecurityKey(key)
        }) {}
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

public readonly record struct ClaimsSecurityDescriptorInput(IDictionary<string, object> Claims, TimeSpan Expiration)
{
    public ClaimsSecurityDescriptorInput(IDictionary<string, object> Claims) : this(Claims, TimeSpan.FromMinutes(5)) {}
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
            Expires            = _time.Get().Add(expiration).DateTime,
            SigningCredentials = _credentials
        };
    }
}