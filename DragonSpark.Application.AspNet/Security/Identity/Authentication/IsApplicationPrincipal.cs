using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class IsApplicationPrincipal : ICondition<ClaimsPrincipal>
{
	public static IsApplicationPrincipal Default { get; } = new();

	IsApplicationPrincipal() : this(IdentityConstants.ApplicationScheme) {}

	readonly string _scheme;

	public IsApplicationPrincipal(string scheme) => _scheme = scheme;

	public bool Get(ClaimsPrincipal parameter) => parameter.Identity?.AuthenticationType == _scheme;
}