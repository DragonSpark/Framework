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
	readonly IAltering<IJSObjectReference> _instance;

	public ResizeMonitors(LoadModule<ResizeMonitor> load) : this(load, ResizeMonitorInstance.Default) {}

	public ResizeMonitors(LoadModule<ResizeMonitor> load, IAltering<IJSObjectReference> instance)
	{
		_load     = load;
		_instance = instance;
	}

	public async ValueTask<IResizeMonitor> Get(EventCallback<ushort> parameter)
	{
		var module    = await _load.Await();
		var reference = await _instance.Await(module);
		return new ResizeMonitor(module, reference, parameter);
	}
}