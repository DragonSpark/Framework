using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Application.Diagnostics
{
	sealed class ExceptionLogger : IExceptionLogger
	{
		readonly ILoggerFactory _factory;

		public ExceptionLogger(ILoggerFactory factory) => _factory = factory;

		public ValueTask<Exception> Get((Type Owner, Exception Exception) parameter)
		{
			var (owner, exception) = parameter;

			var logger = _factory.CreateLogger(owner);

			if (exception is TemplateException template)
			{
				var result = template.InnerException ?? template;
				logger.LogError(result.Demystify(), template.Message, template.Parameters.Open());
				return result.ToOperation();
			}

			logger.LogError(exception.Demystify(), "A problem was encountered while performing this operation.");
			return exception.ToOperation();
		}
	}
}