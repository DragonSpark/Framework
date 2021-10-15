using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public class OtherwiseThrowInputContext<TIn, TOut> : InputOtherwiseContext<TIn, TOut>
{
	public OtherwiseThrowInputContext(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
		: base(subject, condition) {}

	public InputOtherwiseThrowContext<TIn, TOut> Throw<TException>() where TException : Exception
		=> new InputOtherwiseThrowContext<TIn, TOut>(this, x => new Guard<TIn, TException>(Is.Always<TIn>(), x));
}