using DragonSpark.Application.Communication;
using DragonSpark.Model.Commands;
using System.Net.Http;

namespace DragonSpark.Application.Compose.Communication;

public sealed class ApplyState : ICommand<HttpClient>
{
	readonly IClientStateValues _state;
	readonly string             _name;

	public ApplyState(IClientStateValues context) : this(context, CookieHeader.Default) {}

	public ApplyState(IClientStateValues state, string name)
	{
		_state = state;
		_name  = name;
	}

	public void Execute(HttpClient parameter)
	{
		parameter.DefaultRequestHeaders.Add(_name, _state.Get().Open());
	}
}