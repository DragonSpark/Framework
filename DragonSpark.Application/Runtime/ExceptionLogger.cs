﻿using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Application.Runtime
{
	sealed class ExceptionLogger : IExceptionLogger
	{
		readonly ILoggerFactory _factory;

		public ExceptionLogger(ILoggerFactory factory) => _factory = factory;

		public ValueTask Get((Type Owner, Exception Exception) parameter)
		{
			var (owner, exception) = parameter;

			var logger = _factory.CreateLogger(owner);

			if (exception is TemplateException template)
			{
				logger.LogError(template.InnerException!.Demystify(), template.Message, template.Parameters.Open());
			}
			else
			{
				logger.LogError(exception.Demystify(), "A problem was encountered while performing this operation.");
			}

			return Task.CompletedTask.ToOperation();
		}
	}
}