using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Application.Diagnostics;

sealed class LogTemplateException : ISelecting<LogExceptionInput, Exception?>
{
	public static LogTemplateException Default { get; } = new();

	LogTemplateException() : this("A problem was encountered and has been logged for review.") {}

	readonly string _message;

	public LogTemplateException(string message) => _message = message;

	public ValueTask<Exception?> Get(LogExceptionInput parameter)
	{
		var (logger, exception) = parameter;
		var template = exception as TemplateException;
		while (template != null)
		{
			var inner = template.InnerException;
			if (inner != null && inner is not TemplateException)
			{
				// ReSharper disable once TemplateIsNotCompileTimeConstantProblem
				logger.LogError(inner.Demystify(), template.Message, template.Parameters.Open());
				return inner.Account().ToOperation();
			}
			// ReSharper disable once TemplateIsNotCompileTimeConstantProblem
			logger.LogError(template.Message, template.Parameters.Open());

			template = inner as TemplateException;
			if (template is null)
			{
				return new InvalidOperationException(_message, exception).ToOperation<Exception?>();
			}
		}

		return default(Exception?).ToOperation();
	}
}