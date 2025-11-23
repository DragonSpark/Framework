using DragonSpark.Application.Model.Sequences;
using DragonSpark.Model;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Model;

public class OptionCollection<T> : SelectedCollection<Option<T>>
{
	readonly IEqualityComparer<T> _equality;

	public OptionCollection() : this(Empty.Array<Option<T>>()) {}

	public OptionCollection(IEnumerable<Option<T>> list) : this(list, EqualityComparer<T>.Default) {}

	public OptionCollection(IEqualityComparer<T> equality) : this(Empty.Array<Option<T>>(), equality) {}

	public OptionCollection(IEnumerable<Option<T>> list, IEqualityComparer<T> equality)
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
				Selected = null;
			}


		}
	}
}