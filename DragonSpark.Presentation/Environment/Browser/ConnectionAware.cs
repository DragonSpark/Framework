using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public class ConnectionAware : IOperation
{
	readonly IOperation _previous;

	protected ConnectionAware(IOperation previous) => _previous = previous;

	public async ValueTask Get()
	{
		try
		{
			await _previous.Await();
		}
		catch (JSDisconnectedException) {}
		catch (TaskCanceledException) {}
	}
}

public class ConnectionAware<T> : IOperation<T>
{
	readonly IOperation<T> _previous;

	protected ConnectionAware(IOperation<T> previous) => _previous = previous;

	public async ValueTask Get(T parameter)
	{
		try
		{
			await _previous.Await(parameter);
		}
		catch (JSDisconnectedException) {}
		catch (TaskCanceledException) {}
	}
}