using DragonSpark.Application.AspNet.Security.Identity.Model;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class SignOutUser : ICommand
{
	readonly AuthenticationStore _store;
	readonly AuthenticationState _default;
	readonly IRedirectToSignOut  _redirect;

	public SignOutUser(AuthenticationStore store, IDefaultState state, IRedirectToSignOut redirect)
		: this(store, state.Get(), redirect) {}

	public SignOutUser(AuthenticationStore store, AuthenticationState @default, IRedirectToSignOut redirect)
	{
		_store    = store;
		_default  = @default;
		_redirect = redirect;
	}

	public void Execute(None parameter)
	{
		_store.Execute(_default);
		_redirect.Execute();
	}
}