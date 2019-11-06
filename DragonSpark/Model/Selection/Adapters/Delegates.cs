using System;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Runtime;

namespace DragonSpark.Model.Selection.Adapters
{
	sealed class Delegates<T> : ReferenceValueStore<ISelect<None, T>, Func<T>>
	{
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : base(x => x.Get) {}
	}
}