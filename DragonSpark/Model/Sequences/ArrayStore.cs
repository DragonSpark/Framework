using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences
{
	public class ArrayStore<T> : OpenArray<T>
	{
		public ArrayStore(Func<T[]> source) : base(Start.A.Result<T[]>().By.Calling(source).Singleton()) {}
	}

	public class ArrayStore<_, T> : OpenArray<_, T>
	{
		public ArrayStore(ISelect<_, IEnumerable<T>> source) : this(source.Then().Open()) {}

		public ArrayStore(ISelect<_, T[]> source) : this(source.Get) {}

		public ArrayStore(Func<_, T[]> source) : base(new Store<_, T[]>(source).Get) {}
	}
}