using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	[UsedImplicitly]
	sealed class CreateUser<T> : ICreateUser<T> where T : IdentityUser
	{
		readonly IIdentityOperation<T>      _operation;
		readonly Func<ExternalLoginInfo, T> _create;

		public CreateUser(IIdentityOperation<T> operation, Func<ExternalLoginInfo, T> create)
		{
			_operation = operation;
			_create    = create;
		}

		public async ValueTask<CreateUserResult<T>> Get(ExternalLoginInfo parameter)
		{
			var user      = _create(parameter);
			var operation = await _operation.Get(parameter, user);
			var result    = new CreateUserResult<T>(user, operation);
			return result;
		}
	}
}