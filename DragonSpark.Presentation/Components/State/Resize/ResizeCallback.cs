using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State.Resize;

sealed class ResizeCallback : IOperation<ushort>
{
	readonly EventCallback<ushort> _callback;

	public ResizeCallback(EventCallback<ushort> callback) => _callback = callback;

	[JSInvokable("UpdateSize")]
	public ValueTask Get(ushort parameter) => _callback.InvokeAsync(parameter).ToOperation();
}