using DragonSpark.Diagnostics.Logging.Text;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Logging;
using Polly;
using System;

namespace DragonSpark.Diagnostics
{
	public class LogRetryException : ICommand<(Exception, TimeSpan)>
	{
		readonly ILogger _logger;
		readonly string  _template;

		public LogRetryException(ILogger logger) : this(logger, OutputTemplate.Default) {}

		public LogRetryException(ILogger logger, string template)
		{
			_logger   = logger;
			_template = template;
		}

		public void Execute((Exception, TimeSpan) parameter)
		{
			_logger.LogError(parameter.Item1, _template);
		}
	}

	public class LogRetryException<T> : ICommand<(DelegateResult<T>, TimeSpan, Context)>
	{
		readonly ILogger _logger;
		readonly string  _template;

		public LogRetryException(ILogger logger) : this(logger, OutputTemplate.Default) {}

		public LogRetryException(ILogger logger, string template)
		{
			_logger   = logger;
			_template = template;
		}

		public void Execute((DelegateResult<T>, TimeSpan, Context) parameter)
		{
			_logger.LogError(parameter.Item1.Exception, _template);
		}
	}
}