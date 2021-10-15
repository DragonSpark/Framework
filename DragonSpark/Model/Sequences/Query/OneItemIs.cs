using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Invocation;
using System;

namespace DragonSpark.Model.Sequences.Query;

public sealed class OneItemIs<T> : Condition<T[]>, IActivateUsing<Func<T, bool>>
{
	public OneItemIs(Func<T, bool> specification) : this(new Predicate<T>(specification)) {}

	public OneItemIs(Predicate<T> specification)
		: base(new Invocation0<T[], Predicate<T>, bool>(System.Array.Exists, specification).Get) {}
}