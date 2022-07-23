using DragonSpark.Model.Commands;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http.Connections.Client;

namespace DragonSpark.Application.Connections;

public class AssignHeader : ICommand<HttpConnectionOptions>
{
	readonly string                            _key;
	readonly IFormatter<HttpConnectionOptions> _value;

	protected AssignHeader(string key, IFormatter<HttpConnectionOptions> value)
	{
		_key   = key;
		_value = value;
	}

	public void Execute(HttpConnectionOptions parameter)
	{
		parameter.Headers[_key] = _value.Get(parameter);
	}
}