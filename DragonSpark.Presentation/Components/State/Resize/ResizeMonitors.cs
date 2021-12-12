using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Browser;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State.Resize;

sealed class ResizeMonitors : ISelecting<EventCallback<ushort>, IResizeMonitor>
{
	readonly LoadModule<ResizeMonitor>     _load;
	readonly IExceptionLogger              _logger;
	readonly IAltering<IJSObjectReference> _instance;

	public ResizeMonitors(LoadModule<ResizeMonitor> load, IExceptionLogger logger)
		: this(load, logger, ResizeMonitorInstance.Default) {}

	public ResizeMonitors(LoadModule<ResizeMonitor> load, IExceptionLogger logger,
	                      IAltering<IJSObjectReference> instance)
	{
		_load     = load;
		_logger   = logger;
		_instance = instance;
	}

	public async ValueTask<IResizeMonitor> Get(EventCallback<ushort> parameter)
	{
		var module    = await _load.Await();
		var reference = await _instance.Await(module);
		var monitor   = new ResizeMonitor(module, reference, parameter);
		var result    = new ConnectionAwareResizeMonitor(monitor, _logger);
		return result;
	}
}