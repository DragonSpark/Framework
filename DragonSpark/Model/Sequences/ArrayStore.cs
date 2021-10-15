using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences;

public class ArrayStore<T> : OpenArray<T>
{
	protected ArrayStore(Func<T[]> source) : base(Start.A.Result<T[]>().By.Calling(source).Singleton()) {}
}

public class ArrayStore<_, T> : OpenArray<_, T>
{
	protected ArrayStore(ISelect<_, IEnumerable<T>> source) : this(source.Then().Open()) {}

	protected ArrayStore(ISelect<_, Array<T>> source) : this(source.Then().Open()) {}

	protected ArrayStore(ISelect<_, T[]> source) : this(source.Get) {}

	protected ArrayStore(Func<_, T[]> source) : base(new Store<_, T[]>(source).Get) {}
}