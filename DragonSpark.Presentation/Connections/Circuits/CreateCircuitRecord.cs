using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class CreateCircuitRecord : ISelecting<Circuit, CircuitRecord>
{
	readonly NavigationManager           _navigation;
	readonly AuthenticationStateProvider _authentication;
	readonly CurrentReferrer             _referrer;

	public CreateCircuitRecord(NavigationManager navigation, AuthenticationStateProvider authentication,
	                           CurrentReferrer referrer)
	{
		_navigation     = navigation;
		_authentication = authentication;
		_referrer       = referrer;
	}

	public async ValueTask<CircuitRecord> Get(Circuit parameter)
	{
		var referrer = _referrer.Get();
		var state    = await _authentication.GetAuthenticationStateAsync().ConfigureAwait(false);
		return new(parameter, _navigation, state.User, referrer);
	}
}