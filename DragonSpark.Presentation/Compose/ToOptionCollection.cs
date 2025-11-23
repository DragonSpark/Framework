using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Model;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Compose;

sealed class ToOptionCollection<T> : ISelect<Memory<Model.Option<T>>, OptionCollection<T>>
{
	public static ToOptionCollection<T> Default { get; } = new();

	ToOptionCollection() : this(EqualityComparer<T>.Default) {}

	readonly IEqualityComparer<T> _comparer;

	public ToOptionCollection(IEqualityComparer<T> comparer) => _comparer = comparer;

	public OptionCollection<T> Get(Memory<Model.Option<T>> parameter)
	{
		var source = parameter.Span;
		var result = new OptionCollection<T>(_comparer) { Capacity = source.Length };

		foreach (var item in source.AsValueEnumerable())
		{
			result.Add(item);
		}

		return result;
	}
}