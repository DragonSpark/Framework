﻿using DragonSpark.Application.Runtime;
using DragonSpark.Model;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Model;

public class SelectionListingCollection<T> : SelectedCollection<SelectionListing<T>>
{
	readonly IEqualityComparer<T> _equality;

	public SelectionListingCollection() : this(Empty.Array<SelectionListing<T>>()) {}

	public SelectionListingCollection(IEnumerable<SelectionListing<T>> list)
		: this(list, EqualityComparer<T>.Default) {}

	public SelectionListingCollection(IEqualityComparer<T> equality)
		: this(Empty.Array<SelectionListing<T>>(), equality) {}

	public SelectionListingCollection(IEnumerable<SelectionListing<T>> list, IEqualityComparer<T> equality)
		: base(list) => _equality = equality;

	public T? Value
	{
		get => Selected != null ? Selected.Value : default;
		set
		{
			if (value is not null)
			{
				foreach (var listing in this)
				{
					if (_equality.Equals(listing.Value, value))
					{
						Selected = listing;
					}
				}
			}
			else
			{
				Selected = default;
			}


		}
	}
}