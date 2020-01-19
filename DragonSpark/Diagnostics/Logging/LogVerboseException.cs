using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Diagnostics.Logging
{
	public class LogVerboseException : ILogException<Array<object>>
	{
		readonly Exception _action;
		readonly string    _messageTemplate;

		public LogVerboseException(ILogger logger, string messageTemplate) : this(logger.LogTrace<object[]>,
		                                                                          messageTemplate) {}

		public LogVerboseException(Exception action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<Array<object>> parameter)
		{
			_action(parameter.Exception, _messageTemplate, parameter.Argument);
		}
	}

	public class LogVerboseException<T> : ILogException<T>
	{
		readonly Exception<T> _action;
		readonly string       _messageTemplate;

		public LogVerboseException(ILogger logger, string messageTemplate) : this(logger.LogTrace, messageTemplate) {}

		public LogVerboseException(Exception<T> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<T> parameter)
		{
			_action(parameter.Exception, _messageTemplate, parameter.Argument);
		}
	}

	public class LogVerboseException<T1, T2> : ILogException<(T1, T2)>
	{
		readonly Exception<T1, T2> _action;
		readonly string            _messageTemplate;

		public LogVerboseException(ILogger logger, string messageTemplate) : this(logger.LogTrace, messageTemplate) {}

		public LogVerboseException(Exception<T1, T2> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<(T1, T2)> parameter)
		{
			_action(parameter.Exception, _messageTemplate, parameter.Argument.Item1, parameter.Argument.Item2);
		}
	}

	public class LogVerboseException<T1, T2, T3> : ILogException<(T1, T2, T3)>
	{
		readonly Exception<T1, T2, T3> _action;
		readonly string                _messageTemplate;

		public LogVerboseException(ILogger logger, string messageTemplate) : this(logger.LogTrace, messageTemplate) {}

		public LogVerboseException(Exception<T1, T2, T3> action, string messageTemplate)
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