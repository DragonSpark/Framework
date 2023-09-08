using JetBrains.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class StateViews<T> : IStateViews<T> where T : IdentityUser
{
	readonly IUsers<T> _users;

	[UsedImplicitly]
	public StateViews(IUsers<T> users) => _users = users;

	public async ValueTask<StateView<T>> Get(ClaimsPrincipal parameter)
	{
		using var users = _users.Get();
		var manager = users.Subject;
		var user = parameter.Number() is not null ? await manager.GetUserAsync(parameter).ConfigureAwait(false) : null;
		return new(new(parameter, user),
		           user is not null && manager.SupportsUserSecurityStamp
			           ? user.SecurityStamp
			             ??
			             await manager.GetSecurityStampAsync(user).ConfigureAwait(false)
			           : null);
	}
}