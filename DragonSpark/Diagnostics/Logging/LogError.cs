using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Diagnostics.Logging
{
	public class LogError : ILogMessage<Array<object>>
	{
		readonly Message _action;
		readonly string  _messageTemplate;

		public LogError(ILogger logger, string messageTemplate) : this(logger.LogError<object[]>, messageTemplate) {}

		public LogError(Message action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(Array<object> parameter)
		{
			_action(_messageTemplate, parameter);
		}
	}

	public class LogError<T> : ILogException<T>
	{
		readonly Exception<T> _action;
		readonly string     _messageTemplate;

		public LogError(ILogger logger, string messageTemplate) : this(logger.LogError, messageTemplate) {}

		public LogError(Exception<T> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<T> parameter)
		{
			_action(parameter.Exception, _messageTemplate, parameter.Argument);
		}
	}

	public class LogError<T1, T2> : ILogException<(T1, T2)>
	{
		readonly Exception<T1, T2> _action;
		readonly string          _messageTemplate;

		public LogError(ILogger logger, string messageTemplate) : this(logger.LogError, messageTemplate) {}

		public LogError(Exception<T1, T2> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(ExceptionParameter<(T1, T2)> parameter)
		{
			_action(parameter.Exception, _messageTemplate, parameter.Argument.Item1, parameter.Argument.Item2);
		}
	}

	public class LogError<T1, T2, T3> : ILogException<(T1, T2, T3)>
	{
		readonly Exception<T1, T2, T3> _action;
		readonly string              _messageTemplate;

		public LogError(ILogger logger, string messageTemplate) : this(logger.LogError, messageTemplate) {}

		public LogError(Exception<T1, T2, T3> action, string messageTemplate)
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