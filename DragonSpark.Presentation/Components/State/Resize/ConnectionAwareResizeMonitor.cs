using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State.Resize;

sealed class ConnectionAwareResizeMonitor : IResizeMonitor
{
	readonly IResizeMonitor   _previous;
	readonly IExceptionLogger _logger;

	public ConnectionAwareResizeMonitor(IResizeMonitor previous, IExceptionLogger logger)
	{
		_previous = previous;
		_logger   = logger;
	}

	public async ValueTask<bool> Add(ElementReference element)
	{
		try
		{
			return await _previous.Add(element);
		}
		catch (JSException e)
		{
			await _logger.Await(new ExceptionInput(GetType(), e));
		}

		return false;
	}

	public ValueTask Remove(ElementReference element) => _previous.Remove(element);

	public ValueTask DisposeAsync() => _previous.DisposeAsync();
}