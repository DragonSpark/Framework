using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class AddLoginAwareCreate<T> : ICreate<T> where T : IdentityUser
{
	readonly ICreate<T> _previous;
	readonly IUsers<T>  _users;

	public AddLoginAwareCreate(ICreate<T> previous, IUsers<T> users)
	{
		_previous = previous;
		_users    = users;
	}

	public async ValueTask<IdentityResult> Get(Login<T> parameter)
	{
		var previous = await _previous.Await(parameter);
		if (previous.Succeeded)
		{
			var (login, user) = parameter;
			using var users  = _users.Get();
			var       local  = await users.Subject.FindByIdAsync(user.Id.ToString()).ConfigureAwait(false);
			var       result = await users.Subject.AddLoginAsync(local, login).ConfigureAwait(false);
			return result;
		}

		return previous;
	}
}