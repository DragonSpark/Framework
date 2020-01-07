using DragonSpark.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Compose.Model
{
	sealed class Delegates<T> : ReferenceValueStore<ISelect<None, T>, Func<T>>
	{
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : base(x => x.Get) {}
	}
}