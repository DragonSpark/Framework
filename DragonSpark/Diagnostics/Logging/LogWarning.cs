using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Diagnostics.Logging
{
	public class LogWarning : ILogMessage<Array<object>>
	{
		readonly Message _action;
		readonly string  _messageTemplate;

		public LogWarning(ILogger logger, string messageTemplate) :
			this(logger.LogWarning<object[]>, messageTemplate) {}

		public LogWarning(Message action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(Array<object> parameter)
		{
			_action(_messageTemplate, parameter);
		}
	}

	public class LogWarning<T> : ILogMessage<T>
	{
		readonly Message<T> _action;
		readonly string     _messageTemplate;

		public LogWarning(ILogger logger, string messageTemplate) : this(logger.LogWarning, messageTemplate) {}

		public LogWarning(Message<T> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute(T parameter)
		{
			_action(_messageTemplate, parameter);
		}
	}

	public class LogWarning<T1, T2> : ILogMessage<(T1, T2)>
	{
		readonly Message<T1, T2> _action;
		readonly string          _messageTemplate;

		public LogWarning(ILogger logger, string messageTemplate) : this(logger.LogWarning, messageTemplate) {}

		public LogWarning(Message<T1, T2> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute((T1, T2) parameter)
		{
			_action(_messageTemplate, parameter.Item1, parameter.Item2);
		}
	}

	public class LogWarning<T1, T2, T3> : ILogMessage<(T1, T2, T3)>
	{
		readonly Message<T1, T2, T3> _action;
		readonly string              _messageTemplate;

		public LogWarning(ILogger logger, string messageTemplate) : this(logger.LogWarning, messageTemplate) {}

		public LogWarning(Message<T1, T2, T3> action, string messageTemplate)
		{
			_action          = action;
			_messageTemplate = messageTemplate;
		}

		public void Execute((T1, T2, T3) parameter)
		{
			_action(_messageTemplate, parameter.Item1, parameter.Item2, parameter.Item3);
		}
	}
}