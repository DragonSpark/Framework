using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Model;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Compose
{
	sealed class ToSelectionListingCollection<T>
		: ISelect<Memory<SelectionListing<T>>, SelectionListingCollection<T>>
	{
		public static ToSelectionListingCollection<T> Default { get; } = new();

		ToSelectionListingCollection() : this(EqualityComparer<T>.Default) {}

		readonly IEqualityComparer<T> _comparer;

		public ToSelectionListingCollection(IEqualityComparer<T> comparer) => _comparer = comparer;

		public SelectionListingCollection<T> Get(Memory<SelectionListing<T>> parameter)
		{
			var source      = parameter.Span;
			var result      = new SelectionListingCollection<T>(_comparer) { Capacity = source.Length };

			foreach (var item in source.AsValueEnumerable())
			{
				result.Add(item);
			}

			return result;
		}
	}
}