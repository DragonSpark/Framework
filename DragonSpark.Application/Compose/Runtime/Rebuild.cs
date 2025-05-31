using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Application.Compose.Runtime;

sealed class Rebuild<T> : ISelect<RebuildInput<T>, ICollection<T>>
{
	public static Rebuild<T> Default { get; } = new();

	Rebuild() {}

	public ICollection<T> Get(RebuildInput<T> parameter)
	{
		var (collection, memory) = parameter;
		collection.Clear();
		var span = memory.Span;
		for (var i = 0; i < span.Length; i++)
		{
			collection.Add(span[i]);
		}

		return collection;
	}
}