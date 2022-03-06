using DragonSpark.Compose;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class ConnectionAwareEvaluate : IEvaluate
{
	readonly IEvaluate _evaluate;

	public ConnectionAwareEvaluate(IEvaluate evaluate) => _evaluate = evaluate;

	public async ValueTask Get(string parameter)
	{
		try
		{
			await _evaluate.Await(parameter);
		}
		catch (JSDisconnectedException) {}
		catch (TaskCanceledException) {}
	}
}