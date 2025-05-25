using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

sealed class CreateRequest<T> : ICreateRequest where T : IdentityUser
{
	readonly ICreateExternal<T>                            _create;
	readonly IStopAware<ExternalLoginInfo, IdentityResult> _select;
	readonly SignOutScheme                                 _signOut;

	public CreateRequest(ICreateExternal<T> create, IExternalSignin signin, SignOutScheme signOut)
		: this(create, signin.Then().Select(IdentityResults.Default).Out(), signOut) {}

	public CreateRequest(ICreateExternal<T> create, IStopAware<ExternalLoginInfo, IdentityResult> select,
	                     SignOutScheme signOut)
	{
		_create       = create;
		_select       = select;
		_signOut = signOut;
	}

	public async ValueTask<CreateRequestResult> Get(Stop<ExternalLoginInfo> parameter)
	{
		var create = await _create.Off(parameter);
		var (user, call) = create;
		var result = call.Succeeded ? await _select.Off(parameter) : call;
		if (call.Succeeded)
		{
			await _signOut.Off();
		}
		return new(result, user);
	}
}