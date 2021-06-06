using DragonSpark.Model.Sequences;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	[UsedImplicitly]
	sealed class IdentityOperation<T> : IIdentityOperation<T> where T : class
	{
		readonly Array<IIdentityOperation<T>> _actions;

		public IdentityOperation(CreateUserOperation<T> create, AddLoginOperation<T> login)
			: this(Array.Of<IIdentityOperation<T>>(create, login)) {}

		public IdentityOperation(IIdentityOperation<T>[] actions) => _actions = actions;

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
	}
}