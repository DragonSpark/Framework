using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences
{
	public class ArrayStore<T> : DeferredSingleton<Array<T>>,
	                             IArray<T>,
	                             IActivateUsing<IResult<Array<Type>>>,
	                             IActivateUsing<Func<Array<T>>>
	{
		public ArrayStore(IResult<Array<T>> source) : this(source.Get) {}

		public ArrayStore(Func<Array<T>> source) : base(source) {}
	}

	public class ArrayStore<_, T> : Store<_, Array<T>>, IArray<_, T>
	{
		public ArrayStore(ISelect<_, IEnumerable<T>> source) : this(source.Then().Result()) {}

		public ArrayStore(ISelect<_, Array<T>> source) : this(source.Get) {}

		public ArrayStore(Func<_, Array<T>> source) : base(source) {}
	}
}