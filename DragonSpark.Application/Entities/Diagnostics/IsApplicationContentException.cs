using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Application.Entities.Diagnostics;

sealed class IsApplicationContentException : ICondition<InvalidOperationException>
{
	public static IsApplicationContentException Default { get; } = new IsApplicationContentException();

	IsApplicationContentException()
		: this("An exception was thrown while attempting to evaluate the LINQ query parameter expression") {}

	readonly Array<string> _messages;

	public IsApplicationContentException(params string[] messages) => _messages = messages;

	public bool Get(InvalidOperationException parameter)
	{
		var length = _messages.Length;
		for (byte i = 0; i < length; i++)
		{
			if (parameter.Message.StartsWith(_messages[i]))
			{
				return true;
			}
		}
		return false;
	}
}