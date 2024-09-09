using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public class OtherwiseThrowOutputComposer<TIn, TOut> : OutputOtherwiseComposer<TIn, TOut>
{
	public OtherwiseThrowOutputComposer(ISelect<TIn, TOut> subject, Func<TOut, bool> condition)
		: base(subject, condition) {}

	public OutputOtherwiseThrowComposer<TIn, TOut> Throw<TException>() where TException : Exception
		=> new(this, x => new Guard<TIn, TException>(Is.Always<TIn>(), x));
}