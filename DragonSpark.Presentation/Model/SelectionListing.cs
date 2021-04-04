﻿using DragonSpark.Application.Runtime;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Model
{
	public class SelectionListing : SelectionListing<string> {}

	public class SelectionListing<T>
	{
		public T Value { get; set; } = default!;

		public string Name { get; set; } = default!;

		public string Description { get; set; } = default!;

		public string? Tag { get; set; }

		public string Icon { get; set; } = default!;
	}

	public class SelectionListingCollection<T> : SelectedCollection<SelectionListing<T>>
	{
		readonly IEqualityComparer<T> _equality;

		public SelectionListingCollection(IEnumerable<SelectionListing<T>> list)
			: this(list, EqualityComparer<T>.Default) {}

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
}