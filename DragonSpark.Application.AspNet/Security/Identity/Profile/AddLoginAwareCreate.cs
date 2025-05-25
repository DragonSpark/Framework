using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

sealed class AddLoginAwareCreate<T> : ICreate<T> where T : IdentityUser
{
	readonly ICreate<T> _previous;
	readonly IUsers<T>  _users;

	public AddLoginAwareCreate(ICreate<T> previous, IUsers<T> users)
	{
		_previous = previous;
		_users    = users;
	}

	public async ValueTask<IdentityResult> Get(Stop<Login<T>> parameter)
	{
		var previous = await _previous.Off(parameter);
		if (previous.Succeeded)
		{
			var ((login, user), _) = parameter;
			using var users  = _users.Get();
			var       local  = await users.Subject.FindByIdAsync(user.Id.ToString()).Off();
			var       result = await users.Subject.AddLoginAsync(local.Verify(), login).Off();
			return result;
		}

		return previous;
	}
}