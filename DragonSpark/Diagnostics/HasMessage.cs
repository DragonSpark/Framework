using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Diagnostics;

public class HasMessage : ICondition<Exception>
{
	readonly string _message;

	protected HasMessage(string message) => _message = message;

	public bool Get(Exception parameter) => parameter.Message.StartsWith(_message);
}

public class HasAnyMessage : ICondition<Exception>
{
	readonly byte          _count;
	readonly         Array<string> _messages;

	protected HasAnyMessage(params string[] message) : this((byte)message.Length, message) {}

	protected HasAnyMessage(byte count, params string[] messages)
	{
		_count = count;
		_messages    = messages;
	}

	public bool Get(Exception parameter)
	{
		for (var i = 0; i < _count; i++)
		{
			if (parameter.Message.StartsWith(_messages[i]))
			{
				return true;
			}	
		}
		return false;
	}
}