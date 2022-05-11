using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Model.Sequences;

public class SelectedCollection<T> : List<T>
{
	public SelectedCollection(IEnumerable<T> list) : base(list) { }

	public virtual T? Selected { get; set; }
}

public class SelectedCollection<T, TValue> : SelectedCollection<T>, ICondition<TValue>
{
	readonly Func<T, TValue> _select;
	readonly IEqualityComparer<TValue> _equality;

	protected SelectedCollection(Func<T, TValue> select) : this(Empty.Array<T>(), @select) { }

	protected SelectedCollection(IEnumerable<T> list, Func<T, TValue> select)
		: this(list, select, EqualityComparer<TValue>.Default) { }

	protected SelectedCollection(IEnumerable<T> list, Func<T, TValue> select, IEqualityComparer<TValue> equality)
		: base(list)
	{
		_select = select;
		_equality = equality;
	}

	public TValue? Value
	{
		get => Selected is not null ? _select(Selected) : default;
		set
		{
			if (value is not null)
			{
				foreach (var listing in this)
				{
					if (_equality.Equals(_select(listing), value))
					{
						Selected = listing;
						return;
					}
				}
			}

			Selected = default;
		}
	}

	public bool Get(TValue parameter)
	{
		foreach (var listing in this)
		{
			if (_equality.Equals(_select(listing), parameter))
			{
				return true;
			}
		}

		return false;
	}
}