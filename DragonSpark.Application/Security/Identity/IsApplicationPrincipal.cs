using System.Security.Claims;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Security.Identity;

public sealed class IsApplicationPrincipal : ICondition<ClaimsPrincipal>
{
    public static IsApplicationPrincipal Default { get; } = new();

    IsApplicationPrincipal()
        : this("Identity.Application", "http://schemas.microsoft.com/claims/authnmethodsreferences") {}

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