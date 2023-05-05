using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

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