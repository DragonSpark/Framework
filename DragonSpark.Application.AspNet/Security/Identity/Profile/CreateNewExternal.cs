using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

sealed class CreateNewExternal<T> : IStopAware<ExternalLoginInfo, CreateUserResult<T>> where T : class
{
	readonly ICreate<T> _create;
	readonly INew<T>    _new;

	public CreateNewExternal(INew<T> @new, ICreate<T> create)
	{
		_create = create;
		_new    = @new;
	}

	public async ValueTask<CreateUserResult<T>> Get(Stop<ExternalLoginInfo> parameter)
	{
		var (subject, stop) = parameter;
		var user   = await _new.Off(parameter);
		var result = await _create.Off(new(new(subject, user), stop));
		return new(user, result);
	}
}