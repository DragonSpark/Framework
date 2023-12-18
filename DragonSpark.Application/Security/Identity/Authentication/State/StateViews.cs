using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication.State;

sealed class StateViews<T> : IStateViews<T> where T : IdentityUser
{
	readonly IStateUser<T>               _user;
	readonly Stamp<T>                    _stamp;
	readonly ICondition<ClaimsPrincipal> _application;

	public StateViews(IStateUser<T> user, Stamp<T> stamp) : this(user, stamp, IsApplicationPrincipal.Default) {}

	public StateViews(IStateUser<T> user, Stamp<T> stamp, ICondition<ClaimsPrincipal> application)
	{
		_user        = user;
		_stamp       = stamp;
		_application = application;
	}

	public async ValueTask<StateView<T>> Get(ClaimsPrincipal parameter)
	{
		var user = _application.Get(parameter) ? await _user.Await(parameter) : default;
		return new(new(parameter, user), user is not null ? user.SecurityStamp ?? await _stamp.Await(user) : null);
	}
}