using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Initialization;

sealed class Initialized : IInitialized
{
	readonly IIsInitialized        _initialized;
	readonly IInitializeConnection _initialize;

	public Initialized(IIsInitialized initialized, IInitializeConnection initialize)
	{
		_initialized = initialized;
		_initialize  = initialize;
	}

	public bool Get(HttpContext parameter)
	{
		var result = _initialized.Get(parameter);
		if (!result)
		{
			_initialize.Execute(parameter);
		}

		return result;
	}
}