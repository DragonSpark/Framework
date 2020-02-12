using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class CreateUserActions<T> : IIdentityOperation<T> where T : class
	{
		readonly Array<IIdentityOperation<T>> _actions;

		public CreateUserActions(CreateUserOperation<T> create, AddLoginOperation<T> login,
		                         AddClaimsOperation<T> claims)
			: this(An.Array<IIdentityOperation<T>>(create, login, claims)) {}

		public CreateUserActions(Array<IIdentityOperation<T>> actions) => _actions = actions;

		public async ValueTask<IdentityResult> Get((ExternalLoginInfo Login, T User) parameter)
		{
			var length = _actions.Length;
			for (var i = 0u; i < length; i++)
			{
				var current = await _actions[i].Get(parameter);
				if (!current.Succeeded)
				{
					return current;
				}
			}

			return IdentityResult.Success;
		}
	}
}