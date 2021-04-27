using DragonSpark.Model.Selection.Conditions;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Runtime
{
	public class SelectedStructureCollection<T> : List<T> where T : struct
	{
		public SelectedStructureCollection(IEnumerable<T> list) : base(list) {}

		public virtual T? Selected { get; set; }
	}

	public class SelectedStructureCollection<T, TValue> : SelectedStructureCollection<T>, ICondition<TValue>
		where T : struct
	{
		readonly Func<T, TValue>           _select;
		readonly IEqualityComparer<TValue> _equality;

		public SelectedStructureCollection(IEnumerable<T> list, Func<T, TValue> select)
			: this(list, select, EqualityComparer<TValue>.Default) {}

		public SelectedStructureCollection(IEnumerable<T> list, Func<T, TValue> select,
		                                   IEqualityComparer<TValue> equality)
			: base(list)
		{
			_select   = @select;
			_equality = equality;
		}

		public TValue? Value
		{
			get => Selected is not null ? _select(Selected.Value) : default;
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

}