using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using Microsoft.JSInterop;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public class BrowserCommand<T> : IStopAware<T>
{
	readonly IJSRuntime _runtime;
	readonly string     _name;

	protected BrowserCommand(IJSRuntime runtime, string name)
	{
		_runtime = runtime;
		_name    = name;
	}

	public ValueTask Get(Stop<T> parameter) => _runtime.InvokeVoidAsync(_name, parameter.Token, parameter.Subject);
}

public class BrowserCommand : IStopAware
{
	readonly IJSRuntime _runtime;
	readonly string     _name;

	protected BrowserCommand(IJSRuntime runtime, string name)
	{
		_runtime = runtime;
		_name    = name;
	}

	public ValueTask Get(CancellationToken parameter) => _runtime.InvokeVoidAsync(_name, parameter);
}