using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components;

class Class1 {}

public sealed class ModuleReference : ISelecting<string, IJSObjectReference>
{
	readonly IJSRuntime _runtime;
	readonly string     _import;

	public ModuleReference(IJSRuntime runtime, string import = "import")
	{
		_runtime = runtime;
		_import  = import;
	}

	public ValueTask<IJSObjectReference> Get(string parameter)
		=> _runtime.InvokeAsync<IJSObjectReference>(_import, parameter);
}

public class LoadModule : Resulting<IJSObjectReference>
{
	protected LoadModule(ModuleReference load, Type reference)
		: this(load, $"./_content/{reference.Assembly.GetName().Name}/{reference.Name}.js") {}

	protected LoadModule(ModuleReference load, string path) : base(load.Then().Bind(path)) {}
}

public class LoadModule<T> : LoadModule
{
	public LoadModule(ModuleReference load) : base(load, A.Type<T>()) {}
}

sealed class ResizeMonitorInstance : IAltering<IJSObjectReference>
{
	public static ResizeMonitorInstance Default { get; } = new();

	ResizeMonitorInstance() : this(nameof(ResizeMonitorInstance)) {}

	readonly string _name;

	public ResizeMonitorInstance(string name) => _name = name;

	public ValueTask<IJSObjectReference> Get(IJSObjectReference parameter)
		=> parameter.InvokeAsync<IJSObjectReference>(_name);
}

/*public class DotNetReference<T> : SelectedResult<T, DotNetObjectReference<T>> where T : class
{
	protected DotNetReference(T instance) : this(instance.Start().Get()) {}

	protected DotNetReference(IResult<T> previous) : base(previous.Get, DotNetObjectReference.Create) {}
}

sealed class ResizeMonitorReference : DotNetReference<ResizeCallback>
{
	public ResizeMonitorReference(ResizeCallback previous) : base(previous) {}
}*/

sealed class ResizeCallback : IOperation<ushort>
{
	readonly EventCallback<ushort> _callback;

	public ResizeCallback(EventCallback<ushort> callback) => _callback = callback;

	[JSInvokable("UpdateSize")]
	public ValueTask Get(ushort parameter) => _callback.InvokeAsync(parameter).ToOperation();
}

public interface IResizeMonitor : IAsyncDisposable
{
	ValueTask Add(ElementReference element);

	ValueTask Remove(ElementReference element);
}

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

sealed class ResizeMonitor : IResizeMonitor
{
	readonly IJSObjectReference                    _module;
	readonly IJSObjectReference                    _instance;
	readonly DotNetObjectReference<ResizeCallback> _reference;

	public ResizeMonitor(IJSObjectReference module, IJSObjectReference instance, EventCallback<ushort> callback)
		: this(module, instance, DotNetObjectReference.Create(new ResizeCallback(callback))) {}

	public ResizeMonitor(IJSObjectReference module, IJSObjectReference instance,
	                     DotNetObjectReference<ResizeCallback> reference)
	{
		_module    = module;
		_instance  = instance;
		_reference = reference;
	}

	public ValueTask Add(ElementReference element)
		=> _instance.InvokeVoidAsync(nameof(Add), _reference, element.Id, element);

	public ValueTask Remove(ElementReference element) => _instance.InvokeVoidAsync(nameof(Remove), element.Id);

	public async ValueTask DisposeAsync()
	{
		await _module.DisposeAsync().ConfigureAwait(false);
		_reference.Dispose();
	}
}