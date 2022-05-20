using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Diagnostics;

public class HasMessage : ICondition<Exception>
{
	readonly string _message;

	protected HasMessage(string message) => _message = message;

	public bool Get(Exception parameter) => parameter.Message.StartsWith(_message);
}