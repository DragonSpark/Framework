using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Runtime
{
	public class SelectedCollection<T> : List<T>
	{
		public SelectedCollection(IEnumerable<T> list) : base(list) {}

		public virtual T? Selected { get; set; }
	}

	public class SelectedCollection<T, TValue> : SelectedCollection<T>
	{
		readonly Func<T, TValue>           _select;
		readonly IEqualityComparer<TValue> _equality;

		public SelectedCollection(IEnumerable<T> list, Func<T, TValue> select)
			: this(list, select, EqualityComparer<TValue>.Default) {}

		public SelectedCollection(IEnumerable<T> list, Func<T, TValue> select, IEqualityComparer<TValue> equality)
			: base(list)
		{
			_select   = @select;
			_equality = equality;
		}

		public TValue? Value
		{
			get => Selected != null ? _select(Selected) : default;
			set
			{
				if (value is not null)
				{
					foreach (var listing in this)
					{
						if (_equality.Equals(_select(listing), value))
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