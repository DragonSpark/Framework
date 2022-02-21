using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class ConnectionAwareStoreFocusedElement : IOperation
{
	readonly StoreFocusedElement _previous;

	public ConnectionAwareStoreFocusedElement(StoreFocusedElement previous) => _previous = previous;

	public async ValueTask Get()
	{
		try
		{
			await _previous.Await();
		}
		catch (JSDisconnectedException) {}
	}
}