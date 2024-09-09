using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public class OtherwiseThrowInputComposer<TIn, TOut> : InputOtherwiseComposer<TIn, TOut>
{
	public OtherwiseThrowInputComposer(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
		: base(subject, condition) {}

	public InputOtherwiseThrowComposer<TIn, TOut> Throw<TException>() where TException : Exception
		=> new(this, x => new Guard<TIn, TException>(Is.Always<TIn>(), x));
}