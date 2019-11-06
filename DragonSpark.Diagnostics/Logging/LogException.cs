using System;
using System.Collections.Immutable;
using Serilog;
using DragonSpark.Model.Commands;

namespace DragonSpark.Diagnostics.Logging
{
	public class LogException : ICommand<ExceptionParameter<ImmutableArray<object>>>
	{
		readonly Exception _action;
		readonly string    _messageTemplate;

		public LogException(ILogger logger, string messageTemplate) :
			this(logger.Information, messageTemplate) {}

		public LogException(Exception action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<ImmutableArray<object>> parameter) =>
			_action(parameter.Exception, _messageTemplate, parameter.Argument);
	}

	public class LogException<T> : ICommand<ExceptionParameter<T>>
	{
		readonly Exception<T> _action;
		readonly string       _messageTemplate;

		public LogException(ILogger logger, string messageTemplate)
			: this(logger.Information, messageTemplate) {}

		public LogException(Exception<T> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<T> parameter) =>
			_action(parameter.Exception, _messageTemplate, parameter.Argument);
	}

	public class LogException<T1, T2> : ICommand<ExceptionParameter<(T1, T2)>>
	{
		readonly Exception<T1, T2> _action;
		readonly string            _messageTemplate;

		public LogException(ILogger logger, string messageTemplate) :
			this(logger.Information, messageTemplate) {}

		public LogException(Exception<T1, T2> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<ValueTuple<T1, T2>> parameter) =>
			_action(parameter.Exception, _messageTemplate, parameter.Argument.Item1, parameter.Argument.Item2);
	}

	public class LogException<T1, T2, T3> : ICommand<ExceptionParameter<ValueTuple<T1, T2, T3>>>
	{
		readonly Exception<T1, T2, T3> _action;
		readonly string                _messageTemplate;

		public LogException(ILogger logger, string messageTemplate) : this(logger.Information, messageTemplate) {}

		public LogException(Exception<T1, T2, T3> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<ValueTuple<T1, T2, T3>> parameter)
			=> _action(parameter.Exception, _messageTemplate, parameter.Argument.Item1, parameter.Argument.Item2,
			           parameter.Argument.Item3);
	}
}