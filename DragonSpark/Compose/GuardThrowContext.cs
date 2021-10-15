using DragonSpark.Compose.Model.Commands;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Compose;

public sealed class GuardThrowContext<T, TException> where TException : Exception
{
	readonly Func<T, string> _message;

	public GuardThrowContext(Func<T, string> message) => _message = message;

	public CommandContext<T> WhenUnassigned() => When(Is.Assigned<T>().Inverse().Out());

	public CommandContext<T> When(ICondition<T> condition) => new Guard<T, TException>(condition, _message).Then();
}