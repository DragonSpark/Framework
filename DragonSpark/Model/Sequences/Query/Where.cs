using System;

namespace DragonSpark.Model.Sequences.Query
{
	sealed class Where<T> : IBody<T>
	{
		readonly uint           _start;
		readonly Assigned<uint> _until, _limit;
		readonly Func<T, bool>  _where;

		public Where(Func<T, bool> where, Selection selection, Assigned<uint> limit)
			: this(where, selection.Start, selection.Length, limit) {}

		// ReSharper disable once TooManyDependencies
		public Where(Func<T, bool> where, uint start, Assigned<uint> until, Assigned<uint> limit)
		{
			_where = where;
			_start = start;
			_until = until;
			_limit = limit;
		}

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var  to    = parameter.Start + parameter.Length;
			var  array = parameter.Array;
			uint count = 0u, start = 0u;
			var  limit = _limit.Or(_until.Or(parameter.Length));
			for (var i = parameter.Start; i < to && count < limit; i++)
			{
				var item = array[i];
				if (_where(item))
				{
					if (start++ >= _start)
					{
						array[count++] = item;
					}
				}
			}

			return new ArrayView<T>(parameter.Array, 0, count);
		}
	}
}