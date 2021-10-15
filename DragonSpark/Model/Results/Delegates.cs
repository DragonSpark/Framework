using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Model.Results;

sealed class Delegates<T> : ReferenceValueStore<IResult<T>, Func<T>>
{
	public static Delegates<T> Default { get; } = new Delegates<T>();

	Delegates() : base(x => x.Get) {}
}