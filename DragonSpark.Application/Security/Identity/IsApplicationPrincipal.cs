using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

public sealed class IsApplicationPrincipal : ICondition<ClaimsPrincipal>
{
	public static IsApplicationPrincipal Default { get; } = new();

	IsApplicationPrincipal()
		: this(WellKnownApplicationSchemes.Default, "http://schemas.microsoft.com/claims/authnmethodsreferences") {}

	readonly Array<string> _schemes;
	readonly string        _amr;

	public IsApplicationPrincipal(Array<string> schemes, string amr)
	{
		_schemes = schemes;
		_amr     = amr;
	}

	public bool Get(ClaimsPrincipal parameter)
	{
		var schemes = _schemes.Open();
		return (parameter.Identity?.AuthenticationType is {} scheme && schemes.Contains(scheme)) ||
		       schemes.Contains(parameter.FindFirstValue(_amr));
	}
}