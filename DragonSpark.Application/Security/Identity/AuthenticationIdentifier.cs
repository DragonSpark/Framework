using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

sealed class AuthenticationIdentifier : ISelect<ClaimsPrincipal, string?>
{
	public static AuthenticationIdentifier Default { get; } = new();

	AuthenticationIdentifier() : this(ClaimTypes.NameIdentifier) {}

	readonly string _claim;

	public AuthenticationIdentifier(string claim) => _claim = claim;

	public string? Get(ClaimsPrincipal parameter)
		=> parameter.Identity?.IsAuthenticated ?? false
			   ? $"{parameter.Identity.AuthenticationType}_{parameter.FindFirstValue(_claim)}"
			   : null;
}