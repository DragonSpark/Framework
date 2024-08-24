using DragonSpark.Application.Communication;
using DragonSpark.Model.Commands;
using System.Net.Http.Headers;

namespace DragonSpark.Presentation.Connections.Circuits;

public sealed class ApplyState : ICommand<HttpRequestHeaders>
{
	readonly ClientState _state;
	readonly string      _name;

	public ApplyState(ClientState state) : this(state, CookieHeaderName.Default) {}

	public ApplyState(ClientState state, string name)
	{
		_state = state;
		_name  = name;
		_state = state;
		_name  = name;
	}

	public void Execute(HttpRequestHeaders parameter)
	{
		var state = _state.Get();
		if (state is { Success: true, Value: not null })
		{
			parameter.Add(_name, state.Value);
		}
	}
}