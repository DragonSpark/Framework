using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class CreateRequest<T> : ICreateRequest where T : IdentityUser
{
	readonly ICreateExternal<T>                            _create;
	readonly ISelecting<ExternalLoginInfo, IdentityResult> _select;

	public CreateRequest(ICreateExternal<T> create, IExternalSignin signin)
		: this(create, signin.Then().Select(IdentityResults.Default).Out()) {}

	public CreateRequest(ICreateExternal<T> create, ISelecting<ExternalLoginInfo, IdentityResult> select)
	{
		_create = create;
		_select = select;
	}

	public async ValueTask<IdentityResult> Get(ExternalLoginInfo parameter)
	{
		var (_, call) = await _create.Get(parameter);
		var result = call.Succeeded ? await _select.Await(parameter) : call;
		return result;
	}
}