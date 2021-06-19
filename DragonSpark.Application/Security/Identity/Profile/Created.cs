using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile
{
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
	sealed class Created<T> : ICreated<T> where T : class
	{
		readonly UserManager<T> _users;

		public Created(UserManager<T> users) => _users = users;

		public ValueTask<IdentityResult> Get(Login<T> parameter) => _users.CreateAsync(parameter.User).ToOperation();
	}
}