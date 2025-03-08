using DragonSpark.Compose;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public class ModuleInstance : IAsyncDisposable
{
	readonly IJSObjectReference _module;
	readonly IJSObjectReference _instance;
	readonly string             _method;

	protected ModuleInstance(IJSObjectReference module, IJSObjectReference instance, string method = "Dispose")
	{
		_module   = module;
		_instance = instance;
		_method   = method;
	}

	public async ValueTask DisposeAsync()
	{
		await _instance.InvokeVoidAsync(_method).Off();
		await _instance.DisposeAsync().Off();
		await _module.DisposeAsync().Off();
	}
}