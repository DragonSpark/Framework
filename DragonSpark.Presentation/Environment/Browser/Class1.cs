using DragonSpark.Model.Operations;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

internal class Class1 {}

public class BrowserCommand<T> : IOperation<T>
{
	readonly IJSRuntime _runtime;
	readonly string     _name;

	protected BrowserCommand(IJSRuntime runtime, string name)
	{
		_runtime = runtime;
		_name    = name;
	}

	public ValueTask Get(T parameter) => _runtime.InvokeVoidAsync(_name, parameter);
}

public class BrowserCommand : IOperation
{
	readonly IJSRuntime _runtime;
	readonly string     _name;

	protected BrowserCommand(IJSRuntime runtime, string name)
	{
		_runtime = runtime;
		_name    = name;
	}

	public ValueTask Get() => _runtime.InvokeVoidAsync(_name);
}