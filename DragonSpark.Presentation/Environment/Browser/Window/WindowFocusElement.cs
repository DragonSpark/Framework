using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Environment.Browser.Document;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Window;

sealed class WindowFocusElement : IAsyncDisposable
{
	readonly IJSObjectReference _instance;
	readonly IOperation         _dispose;

	public WindowFocusElement(IJSObjectReference instance) : this(instance, new DisposeReference(instance)) {}

	public WindowFocusElement(IJSObjectReference instance, IOperation dispose)
	{
		_instance = instance;
		_dispose  = dispose;
	}

	public ValueTask Start() => _instance.InvokeVoidAsync(nameof(Start));

	public async ValueTask DisposeAsync()
	{
		try
		{
			await _instance.InvokeVoidAsync("Stop").On();
		}
		catch (JSDisconnectedException) {}

		await _dispose.Off();
	}
}