﻿using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Diagnostics.Logging
{
	public class LogMessageException : ILogException<Array<object>>
	{
		readonly Exception _action;
		readonly string    _messageTemplate;

		public LogMessageException(ILogger logger, string messageTemplate) : this(logger.LogInformation, messageTemplate) {}

		public LogMessageException(Exception action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<Array<object>> parameter)
		{
			_action(parameter.Exception, _messageTemplate, parameter.Argument);
		}
	}

	public class LogMessageException<T> : ILogException<T>
	{
		readonly Exception<T> _action;
		readonly string       _messageTemplate;

		public LogMessageException(ILogger logger, string messageTemplate) : this(logger.LogInformation, messageTemplate) {}

		public LogMessageException(Exception<T> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<T> parameter)
		{
			_action(parameter.Exception, _messageTemplate, parameter.Argument);
		}
	}

	public class LogMessageException<T1, T2> : ILogException<(T1, T2)>
	{
		readonly Exception<T1, T2> _action;
		readonly string            _messageTemplate;

		public LogMessageException(ILogger logger, string messageTemplate) : this(logger.LogInformation, messageTemplate) {}

		public LogMessageException(Exception<T1, T2> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<(T1, T2)> parameter)
		{
			_action(parameter.Exception, _messageTemplate, parameter.Argument.Item1, parameter.Argument.Item2);
		}
	}

	public class LogMessageException<T1, T2, T3> : ILogException<(T1, T2, T3)>
	{
		readonly Exception<T1, T2, T3> _action;
		readonly string                _messageTemplate;

		public LogMessageException(ILogger logger, string messageTemplate) : this(logger.LogInformation, messageTemplate) {}

		public LogMessageException(Exception<T1, T2, T3> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<(T1, T2, T3)> parameter)
		{
			_action(parameter.Exception, _messageTemplate, parameter.Argument.Item1, parameter.Argument.Item2,
			        parameter.Argument.Item3);
		}
	}
}