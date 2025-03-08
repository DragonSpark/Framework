using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class LogAwareEvaluate : IEvaluate
{
	readonly IEvaluate _previous;

	public LogAwareEvaluate(IEvaluate previous) => _previous = previous;

	public async ValueTask Get(string parameter)
	{
		try
		{
			await _previous.Off(parameter);
		}
		catch (JSException e)
		{
			throw new TemplateException("Could not evaluate {Payload}", e, parameter);
		}
	}
}