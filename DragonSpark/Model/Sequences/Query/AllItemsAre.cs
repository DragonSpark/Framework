using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Invocation;
using System;

namespace DragonSpark.Model.Sequences.Query;

public sealed class AllItemsAre<T> : Condition<T[]>, IActivateUsing<Func<T, bool>>
{
	public AllItemsAre(Func<T, bool> specification) : this(new Predicate<T>(specification)) {}

	public AllItemsAre(Predicate<T> specification)
		: base(new Invocation0<T[], Predicate<T>, bool>(System.Array.TrueForAll, specification).Get) {}
}