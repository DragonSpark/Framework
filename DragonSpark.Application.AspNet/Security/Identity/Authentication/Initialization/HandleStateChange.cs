using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class HandleStateChange : ICommand<Task<AuthenticationState>>, IOperation<Task<AuthenticationState>>
{
	readonly IAdapters                    _adapters;
	readonly IProcessAuthenticationUpdate _update;

	public HandleStateChange(IAdapters adapters, IProcessAuthenticationUpdate update)
	{
		_adapters = adapters;
		_update   = update;
	}

	public ValueTask Get(Task<AuthenticationState> parameter)
	{
		var adapted = _adapters.Get(parameter);
		return _update.Get(adapted);
	}

	public void Execute(Task<AuthenticationState> parameter)
	{
		Get(parameter);
	}
}