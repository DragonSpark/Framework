using DragonSpark.Application.Compose.Store;
using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

sealed class StateViewKey : Key<ClaimsPrincipal>
{
	public static StateViewKey Default { get; } = new();

	StateViewKey() : base(nameof(StateViewKey), x => x.Identifier() ?? string.Empty) {}
}

// TODO

sealed class UserNumber : ISelect<ClaimsPrincipal, uint?>
{
	public static UserNumber Default { get; } = new();

	UserNumber() : this(ClaimTypes.NameIdentifier) {}

	readonly string _claim;

	public UserNumber(string claim) => _claim = claim;

	public uint? Get(ClaimsPrincipal parameter)
	{
		var name = parameter.FindFirstValue(_claim);
		return name is not null && uint.TryParse(name, out var result) ? result : null;
	}
}

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

sealed class UserDisplayName : IFormatter<ClaimsPrincipal>
{
	public static UserDisplayName Default { get; } = new();

	UserDisplayName() : this(UserName.Default, DisplayName.Default) {}

	readonly IFormatter<ClaimsPrincipal> _name;
	readonly string                      _claim;

	public UserDisplayName(IFormatter<ClaimsPrincipal> name, string claim)
	{
		_name  = name;
		_claim = claim;
	}

	public string Get(ClaimsPrincipal parameter) => parameter.FindFirstValue(_claim) ?? _name.Get(parameter);
}

sealed class UserName : IFormatter<ClaimsPrincipal>
{
	public static UserName Default { get; } = new();

	UserName() : this(Anonymous.Default) {}

	readonly string _anonymous;

	public UserName(string anonymous) => _anonymous = anonymous;

	public string Get(ClaimsPrincipal parameter)
		=> parameter.Identity?.IsAuthenticated ?? false
			   ? parameter.Identity?.Name ?? parameter.FindFirstValue(ClaimTypes.Name).Verify()
			   : _anonymous;
}