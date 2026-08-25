using DragonSpark.Model.Operations.Allocated;
using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class AdaptedStateProvider : IAllocatedResult<AuthenticationState>
{
	readonly AuthenticationStateProvider _provider;
	readonly IAdapters                   _adapters;

	public AdaptedStateProvider(AuthenticationStateProvider provider, IAdapters adapters)
	{
		_provider = provider;
		_adapters = adapters;
	}

	public Task<AuthenticationState> Get()
	{
		var previous = _provider.GetAuthenticationStateAsync();
		return _adapters.Get(previous);
	}
}