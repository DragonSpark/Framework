using DragonSpark.Model.Commands;
using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections.Commands
{
	public class InsertIntoList<T> : ICommand<IList<T>>
	{
		readonly Func<IList<T>, int> _index;
		readonly T                   _item;

		public InsertIntoList(T item) : this(item, _ => 0) {}

		public InsertIntoList(T item, Func<IList<T>, int> index)
		{
			_item  = item;
			_index = index;
		}

		public void Execute(IList<T> parameter)
		{
			parameter.Insert(_index(parameter), _item);
		}
	}
}