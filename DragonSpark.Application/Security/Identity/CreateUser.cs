using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	[UsedImplicitly]
	sealed class CreateUser<T> : ICreateUser<T> where T : IdentityUser
	{
		readonly IIdentityOperation<T> _operation;
		readonly INew<T>               _new;

		public CreateUser(IIdentityOperation<T> operation, INew<T> @new)
		{
			_operation = operation;
			_new       = @new;
		}

		public async ValueTask<CreateUserResult<T>> Get(ExternalLoginInfo parameter)
		{
			var user      = await _new.Get(parameter);
			var operation = await _operation.Await(parameter, user);
			var result    = new CreateUserResult<T>(user, operation);
			return result;
		}
	}
}