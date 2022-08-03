﻿using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public class ExceptionLoggingAware<T> : IOperation<T>
{
	readonly ISelect<T, ValueTask> _previous;
	readonly IExceptionLogger      _logger;
	readonly Type?                 _reportedType;

	public ExceptionLoggingAware(ISelect<T, ValueTask> previous, IExceptionLogger logger, Type? reportedType = null)
	{
		_previous     = previous;
		_logger       = logger;
		_reportedType = reportedType;
	}

	public async ValueTask Get(T parameter)
	{
		try
		{
			await _previous.Await(parameter);
		}
		catch (Exception e)
		{
			await _logger.Await(new(_reportedType ?? GetType(), e));
		}
	}
}