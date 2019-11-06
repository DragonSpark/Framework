using Polly;
using Serilog;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Diagnostics
{
	public class LogRetryException : ICommand<(Exception, TimeSpan)>
	{
		readonly ILogger _logger;

		public LogRetryException(ILogger logger) => _logger = logger;

		public void Execute((Exception, TimeSpan) parameter)
		{
			_logger.Error(parameter.Item1, "");
		}
	}

	public class LogRetryException<T> : ICommand<(DelegateResult<T>, TimeSpan, Context)>
	{
		readonly ILogger _logger;

		public LogRetryException(ILogger logger) => _logger = logger;

		public void Execute((DelegateResult<T>, TimeSpan, Context) parameter)
		{
			_logger.Error(parameter.Item1.Exception, "");
		}
	}
}