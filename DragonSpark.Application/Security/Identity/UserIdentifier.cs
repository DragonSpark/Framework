using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

sealed class UserIdentifier : ISelect<ClaimsPrincipal, string?>
{
	public static UserIdentifier Default { get; } = new();

	UserIdentifier() : this(ClaimTypes.NameIdentifier) {}

	readonly string _claim;

	public UserIdentifier(string claim) => _claim = claim;

	public string? Get(ClaimsPrincipal parameter)
		=> parameter.Identity?.IsAuthenticated ?? false
			   ? $"{parameter.Identity.AuthenticationType}_{parameter.FindFirstValue(_claim)}"
			   : null;
}