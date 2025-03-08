using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Runtime;

public sealed class DisposingAny : IOperation<object>
{
	public static DisposingAny Default { get; } = new();

	DisposingAny() : this(DisposeAny.Default) {}

	readonly ICommand<object> _dispose;

	public DisposingAny(ICommand<object> dispose) => _dispose = dispose;

	public async ValueTask Get(object parameter)
	{
		if (parameter is IAsyncDisposable disposable)
		{
			await disposable.DisposeAsync().Off();
		}
		else
		{
			_dispose.Execute(parameter);
		}
	}
}