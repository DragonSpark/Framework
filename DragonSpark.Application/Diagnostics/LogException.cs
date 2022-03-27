using DragonSpark.Compose;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

sealed class LogException : ILogException
{
	public static LogException Default { get; } = new();

	LogException() {}

	public ValueTask<Exception> Get(LogExceptionInput parameter)
	{
		var (logger, exception) = parameter;
		logger.LogError(exception.Demystify(), "A problem was encountered while performing this operation");
		return exception.ToOperation();
	}
}