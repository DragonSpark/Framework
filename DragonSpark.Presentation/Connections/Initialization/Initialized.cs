using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Initialization;

sealed class Initialized : IInitialized
{
	readonly IInitializeConnection _initialize;
	readonly string                _key;

	public Initialized(IInitializeConnection initialize) : this(initialize, ConnectionIdentifierName.Default) {}

	public Initialized(IInitializeConnection initialize, string key)
	{
		_initialize = initialize;
		_key        = key;
	}

	public bool Get(HttpContext parameter)
	{
		var result = parameter.Request.Cookies.ContainsKey(_key);
		if (!result)
		{
			_initialize.Execute(parameter);
		}

		return result;
	}
}