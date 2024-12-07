using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class CreateRequest<T> : ICreateRequest where T : IdentityUser
{
	readonly ICreateExternal<T>                            _create;
	readonly ISelecting<ExternalLoginInfo, IdentityResult> _select;
	readonly SignOutScheme                                 _signOut;

	public CreateRequest(ICreateExternal<T> create, IExternalSignin signin, SignOutScheme signOut)
		: this(create, signin.Then().Select(IdentityResults.Default).Out(), signOut) {}

	public CreateRequest(ICreateExternal<T> create, ISelecting<ExternalLoginInfo, IdentityResult> select,
	                     SignOutScheme signOut)
	{
		_create       = create;
		_select       = select;
		_signOut = signOut;
	}

	public async ValueTask<CreateRequestResult> Get(ExternalLoginInfo parameter)
	{
		var create = await _create.Await(parameter);
		var (user, call) = create;
		var result = call.Succeeded ? await _select.Await(parameter) : call;
		if (call.Succeeded)
		{
			await _signOut.Await();
		}
		return new(result, user);
	}
}