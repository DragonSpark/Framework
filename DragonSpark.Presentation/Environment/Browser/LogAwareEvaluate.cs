using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class LogAwareEvaluate : IEvaluate
{
	readonly IEvaluate _previous;

	public LogAwareEvaluate(IEvaluate previous) => _previous = previous;

	public async ValueTask Get(Stop<string> parameter)
	{
		try
		{
			await _previous.Off(parameter);
		}
		catch (JSException e)
		{
			var (subject, _) = parameter;
			throw new TemplateException("Could not evaluate {Payload}", e, subject);
		}
	}
}