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

	public class LogError<T> : ILogMessage<T>
	{
		readonly Message<T> _action;
		readonly string     _messageTemplate;

		public LogError(ILogger logger, string messageTemplate) : this(logger.LogError, messageTemplate) {}

		public LogError(Message<T> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(T parameter)
		{
			_action(_messageTemplate, parameter);
		}
	}

	public class LogError<T1, T2> : ILogMessage<(T1, T2)>
	{
		readonly Message<T1, T2> _action;
		readonly string          _messageTemplate;

		public LogError(ILogger logger, string messageTemplate) : this(logger.LogError, messageTemplate) {}

		public LogError(Message<T1, T2> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute((T1, T2) parameter)
		{
			var (first, second) = parameter;
			_action(_messageTemplate, first, second);
		}
	}

	public class LogError<T1, T2, T3> : ILogMessage<(T1, T2, T3)>
	{
		readonly Message<T1, T2, T3> _action;
		readonly string              _messageTemplate;

		public LogError(ILogger logger, string messageTemplate) : this(logger.LogError, messageTemplate) {}

		public LogError(Message<T1, T2, T3> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute((T1, T2, T3) parameter)
		{
			var (first, second, third) = parameter;
			_action(_messageTemplate, first, second, third);
		}
	}
}