using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	[UsedImplicitly]
	sealed class CreateUser<T> : ICreateUser<T> where T : IdentityUser
	{
		readonly IIdentityOperation<T>       _actions;
		readonly Func<ExternalLoginInfo, T> _create;

		public CreateUser(IIdentityOperation<T> actions, Func<ExternalLoginInfo, T> create)
		{
			_actions = actions;
			_create  = create;
		}

		public async ValueTask<CreateUserResult<T>> Get(ExternalLoginInfo parameter)
		{
			var user   = _create(parameter);
			var result = new CreateUserResult<T>(user, await _actions.Get((parameter, user)));
			return result;
		}
	}
}