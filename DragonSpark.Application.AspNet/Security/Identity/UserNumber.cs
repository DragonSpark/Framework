using DragonSpark.Application.Security.Identity;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class UserNumber : ISelect<ClaimsPrincipal, uint?>
{
	public static UserNumber Default { get; } = new();

	UserNumber() : this(IsApplicationPrincipal.Default, ClaimTypes.NameIdentifier) {}

	readonly ICondition<ClaimsPrincipal> _application;
	readonly string                      _claim;

	public UserNumber(ICondition<ClaimsPrincipal> application, string claim)
	{
		_application = application;
		_claim       = claim;
	}

	public uint? Get(ClaimsPrincipal parameter)
	{
		if (_application.Get(parameter))
		{
			var name = parameter.FindFirstValue(_claim);
			if (name is not null && uint.TryParse(name, out var result))
			{
				return result;
			}
		}

		return null;
	}
}