using System.Security.Claims;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity;

public sealed class IsApplicationPrincipal : ICondition<ClaimsPrincipal>
{
    public static IsApplicationPrincipal Default { get; } = new();

    IsApplicationPrincipal()
        : this(IdentityConstants.ApplicationScheme, "http://schemas.microsoft.com/claims/authnmethodsreferences") {}

    readonly string _scheme;
    readonly string _amr;

    public IsApplicationPrincipal(string scheme, string amr)
    {
        _scheme   = scheme;
        _amr = amr;
    }

    public bool Get(ClaimsPrincipal parameter)
        => parameter.Identity?.AuthenticationType == _scheme || parameter.FindFirstValue(_amr) == _scheme;
}