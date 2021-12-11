using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State.Resize;

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

	public ValueTask<bool> Add(ElementReference element)
		=> _instance.InvokeAsync<bool>(nameof(Add), _reference, element.Id, element);

	public ValueTask Remove(ElementReference element) => _instance.InvokeVoidAsync(nameof(Remove), element.Id);

	public async ValueTask DisposeAsync()
	{
		await _module.DisposeAsync().ConfigureAwait(false);
		_reference.Dispose();
	}
}