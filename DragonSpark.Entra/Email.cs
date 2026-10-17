using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System.Security.Claims;

namespace DragonSpark.Entra;

sealed class Email : ISelect<ClaimsPrincipal, string?>
{
	public static Email Default { get; } = new();

	Email() : this(ClaimTypes.Email, EmailClaim.Default, "email") {}

	readonly Array<string> _claims;

	public Email(params string[] claims) => _claims = claims;

	public string? Get(ClaimsPrincipal parameter)
	{
		foreach (var claim in _claims)
		{
			if (parameter.FindFirstValue(claim) is {} value)
			{
				return value;
			}
		}

		return null;
	}
}