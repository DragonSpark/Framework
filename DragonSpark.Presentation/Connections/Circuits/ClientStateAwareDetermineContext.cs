using DragonSpark.Application.AspNet.Communication;
using DragonSpark.Compose;
using DragonSpark.Presentation.Environment;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class ClientStateAwareDetermineContext : IDetermineContext
{
	readonly IDetermineContext _previous;
	readonly ClientState       _state;
	readonly IHeader           _header;

	public ClientStateAwareDetermineContext(IDetermineContext previous, ClientState state)
		: this(previous, state, CookieHeader.Default) {}

	public ClientStateAwareDetermineContext(IDetermineContext previous, ClientState state, IHeader header)
	{
		_previous = previous;
		_state    = state;
		_header   = header;
	}

	public HttpContext Get(HttpContext parameter)
	{
		var result  = _previous.Get(parameter);
		var subject = result.Request.Headers;
		var current = _header.Get(subject);
		if (current is null)
		{
			var (success, state) = _state.Get();
			if (success && state is not null)
			{
				var value = !string.IsNullOrEmpty(current) ? $"{current.TrimEnd(';')};{state}" : state;
				_header.Assign(subject, value);
			}
		}

		return result;
	}
}