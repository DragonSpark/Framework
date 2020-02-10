using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security
{
	public class CreateUser<T> : ICreateUser<T> where T : class
	{
		readonly CreateUserActions<T>       _actions;
		readonly Func<ExternalLoginInfo, T> _create;

		public CreateUser(CreateUserActions<T> actions, Func<ExternalLoginInfo, T> create)
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