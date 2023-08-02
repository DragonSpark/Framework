using DragonSpark.Compose;
using System;

namespace DragonSpark.Model.Sequences;

public class ArrayStore<T> : OpenArray<T>
{
	protected ArrayStore(Func<T[]> source) : base(Start.A.Result<T[]>().By.Calling(source).Singleton()) {}
}