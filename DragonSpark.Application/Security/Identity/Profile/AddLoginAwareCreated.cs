using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile
{
	sealed class AddLoginAwareCreated<T> : ICreated<T> where T : class
	{
		readonly ICreated<T>    _previous;
		readonly UserManager<T> _users;

		public AddLoginAwareCreated(ICreated<T> previous, UserManager<T> users)
		{
			_previous = previous;
			_users    = users;
		}

		public async ValueTask<IdentityResult> Get(Login<T> parameter)
		{
			var previous = await _previous.Await(parameter);
			var (login, user) = parameter;
			if (previous.Succeeded)
			{
				return await _users.AddLoginAsync(user, login).ConfigureAwait(false);
			}

			return previous;
		}
	}
}