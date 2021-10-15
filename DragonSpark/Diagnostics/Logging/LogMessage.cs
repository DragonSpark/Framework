using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Diagnostics.Logging;

public class LogMessage : ILogMessage<Array<object>>
{
	readonly Message _action;
	readonly string  _messageTemplate;

	public LogMessage(ILogger logger, string messageTemplate) : this(logger.LogInformation<object[]>,
	                                                                 messageTemplate) {}

	public LogMessage(Message action, string messageTemplate)
	{
		_action          = action;
		_messageTemplate = messageTemplate;
	}

	public void Execute(Array<object> parameter)
	{
		_action(_messageTemplate, parameter);
	}
}

public class LogMessage<T> : ILogMessage<T>
{
	readonly Message<T> _action;
	readonly string     _messageTemplate;

	public LogMessage(ILogger logger, string messageTemplate) : this(logger.LogInformation, messageTemplate) {}

	public LogMessage(Message<T> action, string messageTemplate)
	{
		_action          = action;
		_messageTemplate = messageTemplate;
	}

	public void Execute(T parameter)
	{
		_action(_messageTemplate, parameter);
	}
}

public class LogMessage<T1, T2> : ILogMessage<(T1, T2)>
{
	readonly Message<T1, T2> _action;
	readonly string          _messageTemplate;

	public LogMessage(ILogger logger, string messageTemplate) : this(logger.LogInformation, messageTemplate) {}

	public LogMessage(Message<T1, T2> action, string messageTemplate)
	{
		_action          = action;
		_messageTemplate = messageTemplate;
	}

	public void Execute((T1, T2) parameter)
	{
		_action(_messageTemplate, parameter.Item1, parameter.Item2);
	}
}

public class LogMessage<T1, T2, T3> : ILogMessage<(T1, T2, T3)>
{
	readonly Message<T1, T2, T3> _action;
	readonly string              _messageTemplate;

	public LogMessage(ILogger logger, string messageTemplate) : this(logger.LogInformation, messageTemplate) {}

	public LogMessage(Message<T1, T2, T3> action, string messageTemplate)
	{
		_action          = action;
		_messageTemplate = messageTemplate;
	}

	public void Execute((T1, T2, T3) parameter)
	{
		_action(_messageTemplate, parameter.Item1, parameter.Item2, parameter.Item3);
	}
}