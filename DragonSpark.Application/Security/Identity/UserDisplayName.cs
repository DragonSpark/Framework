using DragonSpark.Text;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

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