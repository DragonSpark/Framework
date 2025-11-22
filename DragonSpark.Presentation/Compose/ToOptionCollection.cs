using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Model;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Presentation.Compose;

sealed class ToOptionCollection<T>
	: ISelect<Memory<Model.Option<T>>, OptionCollection<T>>
{
	public static ToOptionCollection<T> Default { get; } = new();

	ToOptionCollection() {}

	public OptionCollection<T> Get(Memory<Model.Option<T>> parameter)
	{
		var source = parameter.Span;
		var result = new OptionCollection<T>() { Capacity = source.Length };

		foreach (var item in source.AsValueEnumerable())
		{
			result.Add(item);
		}

		return result;
	}
}