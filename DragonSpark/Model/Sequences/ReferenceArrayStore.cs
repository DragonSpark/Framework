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

public class ArrayStore<TIn, T> : OpenArray<TIn, T>
{
	protected ArrayStore(ISelect<TIn, IEnumerable<T>> source) : this(source.Then().Open()) {}

	protected ArrayStore(ISelect<TIn, Array<T>> source) : this(source.Then().Open()) {}

	protected ArrayStore(ISelect<TIn, T[]> source) : this(source.Get) {}

	protected ArrayStore(Func<TIn, T[]> source) : base(new Store<TIn, T[]>(source).Get) {}
}

public class ReferenceArrayStore<TIn, T> : OpenArray<TIn, T> where TIn : class
{
	protected ReferenceArrayStore(ISelect<TIn, IEnumerable<T>> source) : this(source.Then().Open()) {}

	protected ReferenceArrayStore(ISelect<TIn, Array<T>> source) : this(source.Then().Open()) {}

	protected ReferenceArrayStore(ISelect<TIn, T[]> source) : this(source.Get) {}

	protected ReferenceArrayStore(Func<TIn, T[]> source) : base(new ReferenceValueTable<TIn, T[]>(source).Get) {}
}