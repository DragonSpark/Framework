using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class CreateNewExternal<T> : ISelecting<ExternalLoginInfo, CreateUserResult<T>> where T : class
{
	readonly ICreate<T> _create;
	readonly INew<T>    _new;

	public CreateNewExternal(INew<T> @new, ICreate<T> create)
	{
		_create = create;
		_new    = @new;
	}

	public async ValueTask<CreateUserResult<T>> Get(ExternalLoginInfo parameter)
	{
		var user   = await _new.Await(parameter);
		var result = await _create.Await(new(parameter, user));
		return new(user, result);
	}
}