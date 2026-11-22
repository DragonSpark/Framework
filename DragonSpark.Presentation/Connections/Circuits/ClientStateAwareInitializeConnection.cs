using DragonSpark.Application.AspNet.Communication;
using DragonSpark.Model;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class ClientStateAwareInitializeConnection : IInitializeConnection
{
	readonly IInitializeConnection _previous;
	readonly CurrentCookie         _current;
	readonly ClientState           _state;

	public ClientStateAwareInitializeConnection(IInitializeConnection previous, CurrentCookie current,
	                                            ClientState state)
	{
		_previous = previous;
		_current  = current;
		_state    = state;
	}

	public void Execute(None parameter)
	{
		_previous.Execute(parameter);
		if (!_state.Get().Success && _current.Get() is {} current)
		{
			_state.Execute(current);
		}
	}
}