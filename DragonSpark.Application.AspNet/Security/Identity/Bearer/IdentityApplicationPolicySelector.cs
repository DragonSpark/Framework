using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class IdentityApplicationPolicySelector : IPolicySelector
{
    public static IdentityApplicationPolicySelector Default { get; } = new();

    IdentityApplicationPolicySelector()
        : this("Bearer ", JwtBearerDefaults.AuthenticationScheme, IdentityConstants.ApplicationScheme) {}

    readonly string _key, _scheme, _previous;

    public IdentityApplicationPolicySelector(string key, string scheme, string previous)
    {
        _key      = key;
        _scheme   = scheme;
        _previous = previous;
    }

    public string Get(HttpContext parameter)
    {
        var header = parameter.Request.Headers.Authorization;
        var result = !string.IsNullOrEmpty(header) && header.ToString().StartsWith(_key) ? _scheme : _previous;
        return result;
    }
}