using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Diagnostics.Logging
{
	public class LogErrorException : ILogException<Array<object>>
	{
		readonly Exception _action;
		readonly string    _messageTemplate;

		public LogErrorException(ILogger logger, string messageTemplate) : this(logger.LogError<object[]>,
		                                                                        messageTemplate) {}

		public LogErrorException(Exception action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<Array<object>> parameter)
		{
			_action(parameter.Exception, _messageTemplate, parameter.Argument);
		}
	}

	public class LogErrorException<T> : ILogException<T>
	{
		readonly Exception<T> _action;
		readonly string       _messageTemplate;

		public LogErrorException(ILogger logger, string messageTemplate) : this(logger.LogError, messageTemplate) {}

		public LogErrorException(Exception<T> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<T> parameter)
		{
			var (exception, argument) = parameter;
			_action(exception, _messageTemplate, argument);
		}
	}

	public class LogErrorException<T1, T2> : ILogException<(T1, T2)>
	{
		readonly Exception<T1, T2> _action;
		readonly string            _messageTemplate;

		public LogErrorException(ILogger logger, string messageTemplate) : this(logger.LogError, messageTemplate) {}

		public LogErrorException(Exception<T1, T2> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<(T1, T2)> parameter)
		{
			var (exception, (first, second)) = parameter;
			_action(exception, _messageTemplate, first, second);
		}
	}

	public class LogErrorException<T1, T2, T3> : ILogException<(T1, T2, T3)>
	{
		readonly Exception<T1, T2, T3> _action;
		readonly string                _messageTemplate;

		public LogErrorException(ILogger logger, string messageTemplate) : this(logger.LogError, messageTemplate) {}

		public LogErrorException(Exception<T1, T2, T3> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<(T1, T2, T3)> parameter)
		{
			var (exception, (first, second, third)) = parameter;
			_action(exception, _messageTemplate, first, second, third);
		}
	}
}