using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using System.Linq;
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

sealed class PrincipalProviderIdentity : Select<ClaimsPrincipal, ProviderIdentity>
{
	public static PrincipalProviderIdentity Default { get; } = new();

	PrincipalProviderIdentity()
		: base(UserIdentifier.Default.Then().Verified().Select(ProviderIdentityParser.Default)) {}
}

sealed class ProviderIdentityParser : IParser<ProviderIdentity>
{
	public static ProviderIdentityParser Default { get; } = new();

	ProviderIdentityParser() : this('_') {}

	readonly char _token;

	public ProviderIdentityParser(char token) => _token = token;

	public ProviderIdentity Get(string parameter)
	{
		var parts = parameter.Split(_token);
		return new (parts.First(), parts.Last());
	}
}