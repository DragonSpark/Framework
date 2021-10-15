using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose;

public sealed class GuardModelContext<TException> where TException : Exception
{
	public static GuardModelContext<TException> Default { get; } = new GuardModelContext<TException>();

	GuardModelContext() {}

	public GuardThrowContext<T, TException> Displaying<T>(ISelect<T, string> message) => Displaying<T>(message.Get);

	public GuardThrowContext<T, TException> Displaying<T>(Func<T, string> message)
		=> new GuardThrowContext<T, TException>(message);
}