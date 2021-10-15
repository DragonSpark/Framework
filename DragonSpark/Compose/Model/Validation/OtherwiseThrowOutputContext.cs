using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public class OtherwiseThrowOutputContext<TIn, TOut> : OutputOtherwiseContext<TIn, TOut>
{
	public OtherwiseThrowOutputContext(ISelect<TIn, TOut> subject, Func<TOut, bool> condition)
		: base(subject, condition) {}

	public OutputOtherwiseThrowContext<TIn, TOut> Throw<TException>() where TException : Exception
		=> new OutputOtherwiseThrowContext<TIn, TOut>(this, x => new Guard<TIn, TException>(Is.Always<TIn>(), x));
}