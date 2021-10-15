using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

/*[UsedImplicitly]
sealed class Created<T> : ICreated<T> where T : class
{
	readonly Array<ICreated<T>> _actions;

	public Created(CreateUserOperation<T> create, AddLoginOperation<T> login)
		: this(Array.Of<ICreated<T>>(create, login)) {}

	public Created(ICreated<T>[] actions) => _actions = actions;

	public async ValueTask<IdentityResult> Get((ExternalLoginInfo Login, T User) parameter)
	{
		var length = _actions.Length;
		for (var i = 0; i < length; i++)
		{
			var current = await _actions[i].Get(parameter);
			if (!current.Succeeded)
			{
				return current;
			}
		}

		return IdentityResult.Success;
	}
}*/
sealed class Create<T> : ICreate<T> where T : class
{
	readonly IUsers<T> _users;

	public Create(IUsers<T> users) => _users = users;

	public async ValueTask<IdentityResult> Get(Login<T> parameter)
	{
		using var users  = _users.Get();
		var       result = await users.Subject.CreateAsync(parameter.User).ConfigureAwait(false);
		return result;
	}
}